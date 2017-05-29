// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace SolidCP.Ecommerce.EnterpriseServer
{
	/// <summary>
	/// Payment provider for the PayPal Gateway 
	/// </summary>
	public class PayPalStandardProvider : SystemPluginBase, IInteractivePaymentGatewayProvider
	{
		public const string DEMO_SERVICE_URL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
		public const string LIVE_SERVICE_URL = "https://www.paypal.com/cgi-bin/webscr";

		public const string BUSINESS_NOT_MATCH_MSG = "Business account doesn't match";
		public const string INVALID_RESPONSE_MSG = "Response is reported as either invalid or corrupted";
		public const string PAYMENT_REJECTED_MSG = "Payment has been rejected by payment gateway";
		public const string IPN_PROCESSOR_ENDPOINT = "Services/PayPalIPN.ashx";

		public const string CART_COMMAND = "_cart";
		public const string ErrorPrefix = "PayPalStandard.";

		/// <summary>
		/// Gets whether plug-in running in live mode
		/// </summary>
		public bool LiveMode
		{
			get
			{
				return Convert.ToBoolean(PluginSettings[PayPalStdSettings.LIVE_MODE]);
			}
		}
		/// <summary>
		/// Gets service business account
		/// </summary>
		public string Business
		{
			get { return PluginSettings[PayPalStdSettings.BUSINESS]; }
		}
		/// <summary>
		/// Gets service return url
		/// </summary>
		public string ReturnUrl
		{
			get { return PluginSettings[PayPalStdSettings.RETURN_URL]; }
		}
		/// <summary>
		/// Gets service cancel return url
		/// </summary>
		public string CancelReturnUrl
		{
			get { return PluginSettings[PayPalStdSettings.CANCEL_RETURN_URL]; }
		}
		/// <summary>
		/// Gets service url
		/// </summary>
		public string ServiceUrl
		{
			get
			{
				if (LiveMode)
					return LIVE_SERVICE_URL;

				return DEMO_SERVICE_URL;
			}
		}

		public PayPalStandardProvider()
		{
		}

		#region IInteractivePaymentGatewayProvider Members

		public CheckoutFormParams GetCheckoutFormParams(FormParameters formParams, InvoiceItem[] invoiceLines)
		{
			// normalize target site variable
			string targetSite = formParams["target_site"].EndsWith("/") ? formParams["target_site"] 
				: formParams["target_site"] + "/";

			CheckoutFormParams outputParams = new CheckoutFormParams();
			// setup service url
			outputParams.Action = ServiceUrl;
			outputParams.Method = CheckoutFormParams.POST_METHOD;
			// copy command
			outputParams["cmd"] = CART_COMMAND;
			// workaround to make IPN callbacks work
			outputParams["rm"] = "2";
			// set upload value to indicate a third-party shopping cart
			outputParams["upload"] = "1";
			// set business
			outputParams["business"] = Business;
			// set cancel return url
			outputParams["cancel_return"] = String.Concat(targetSite, "?pid=ecOnlineStore&ContractId=", formParams[FormParameters.CONTRACT]);
			// set return url
			outputParams["return"] = targetSite;
			// set IPN-Endpoint url
			outputParams["notify_url"] = String.Concat(targetSite, IPN_PROCESSOR_ENDPOINT);
			// set invoice number
			outputParams["invoice"] = formParams[FormParameters.INVOICE];
			// set contract number
			outputParams["custom"] = formParams[FormParameters.CONTRACT];
			// set invoice currency
			outputParams["currency_code"] = formParams[FormParameters.CURRENCY];
			// set formatted invoice number
			outputParams["item_name_1"] = String.Format("{0} #{1}", FormParameters.INVOICE, formParams[FormParameters.INVOICE]);
			// set invoice amount
			outputParams["amount_1"] = formParams[FormParameters.AMOUNT];
			// copy first name
			AddParameter(formParams, outputParams, FormParameters.FIRST_NAME, "first_name");
			// copy last name
			AddParameter(formParams, outputParams, FormParameters.LAST_NAME, "last_name");
			// copy address
			AddParameter(formParams, outputParams, FormParameters.ADDRESS, "address1");
			// copy city
			AddParameter(formParams, outputParams, FormParameters.CITY, "city");
			// copy state
			AddParameter(formParams, outputParams, FormParameters.STATE, "state");
			// copy country
			AddParameter(formParams, outputParams, FormParameters.COUNTRY, "country");
			// copy zip
			AddParameter(formParams, outputParams, FormParameters.ZIP, "zip");
			// copty email
			AddParameter(formParams, outputParams, FormParameters.EMAIL, "email");
			// copy phone number
			if (formParams[FormParameters.COUNTRY] != "US" && formParams[FormParameters.COUNTRY] != "CA")
			{
				// phone numbers outside U.S copy as is
				outputParams["night_phone_b"] = formParams[FormParameters.PHONE];
			}
			// return whole set of params
			return outputParams;
		}
		#endregion

		#region IPaymentGatewayProvider Members

		public TransactionResult SubmitPaymentTransaction(CheckoutDetails details)
		{
			//init result structure
			TransactionResult result = new TransactionResult();
			// check is request genuine depending on the provider mode
			Process_IPN_Request(result, details);
			//
			return result;
		}

		private void Process_PDT_Request(TransactionResult result, CheckoutDetails details)
		{

		}

		private void Process_IPN_Request(TransactionResult result, CheckoutDetails details)
		{
			result.RawResponse = "";

			// build raw response
			foreach (string keyName in details.GetAllKeys())
			{
				if (String.IsNullOrEmpty(keyName))
					continue;

				// check for separator
				if (!String.IsNullOrEmpty(result.RawResponse) && 
					!result.RawResponse.EndsWith("&"))
					result.RawResponse += "&";

				result.RawResponse += keyName + "=" + details[keyName];
			}
			// compare business account against email addres in response
			if (!String.Equals(details["receiver_email"], Business, StringComparison.InvariantCultureIgnoreCase))
				throw new Exception(BUSINESS_NOT_MATCH_MSG);

			// validate whether response still genuine
			if(!IsResponseGenuine(result.RawResponse))
				throw new Exception(INVALID_RESPONSE_MSG);
			// build tran id
			string transactionId = details["txn_id"];
			// check payment status
			switch(details["payment_status"])
			{
				case "Completed":
				case "Processed":
					result.Succeed = true;
					// store order details
					result.TransactionId = transactionId;
					result.TransactionStatus = TransactionStatus.Approved;
					break;
				case "Pending":
					result.Succeed = true;
					// store order details
					result.TransactionId = transactionId;
					result.TransactionStatus = TransactionStatus.Pending;
					break;
				default:
					result.Succeed = false;
					result.TransactionStatus = TransactionStatus.Declined;
					break;
			}
		}
		#endregion

		private void AddParameter(FormParameters inputParams, CheckoutFormParams outputParams, string inputKey, string outputKey)
		{
			string formParameter = inputParams[inputKey];
			if (formParameter != null)
			{
				outputParams[outputKey] = formParameter;
			}
		}
		
		private bool IsResponseGenuine(string strFormValues)
		{
			// Create the request back
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServiceUrl);

			// Set values for the request back
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			string strNewValue = strFormValues + "&cmd=_notify-validate";
			request.ContentLength = strNewValue.Length;
			// Write the request back IPN strings
			StreamWriter writer = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
			writer.Write(strNewValue);
			writer.Close();

			// Do the request to PayPal and get the response
			StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
			string strResponse = reader.ReadToEnd().Trim().ToUpper();
			reader.Close();

			return String.Equals(strResponse, "VERIFIED", StringComparison.InvariantCultureIgnoreCase);
		}

		private string HtmlEncode(string s)
		{
			return HttpContext.Current.Server.UrlEncode(s);
		}
	}
}

