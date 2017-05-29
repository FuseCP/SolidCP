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
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace SolidCP.Ecommerce.EnterpriseServer
{
	/// <summary>
	/// Payment provider for the PayPal Gateway 
	/// </summary>
	public class PayPalProProvider : SystemPluginBase, IPaymentGatewayProvider
	{
		public const string DEMO_SERVICE_URL = "https://api-aa.sandbox.paypal.com/2.0/";
		public const string LIVE_SERVICE_URL = "https://api-aa-3t.paypal.com/2.0/";
		public const string PROCESSOR_VERSION = "2.0";

		public override string[] SecureSettings
		{
			get
			{
				return new string[] { PayPalProSettings.SIGNATURE, PayPalProSettings.PASSWORD };
			}
		}

		/// <summary>
		/// Gets whether plug-in running in live mode
		/// </summary>
		public bool LiveMode
		{
			get
			{
				return Convert.ToBoolean(PluginSettings[PayPalProSettings.LIVE_MODE]);
			}
		}
		/// <summary>
		/// Gets service username
		/// </summary>
		public string Username
		{
			get { return PluginSettings[PayPalProSettings.USERNAME]; }
		}
		/// <summary>
		/// Gets service password
		/// </summary>
		public string Password
		{
			get { return PluginSettings[PayPalProSettings.PASSWORD]; }
		}
		/// <summary>
		/// Gets service signature
		/// </summary>
		public string Signature
		{
			get { return PluginSettings[PayPalProSettings.SIGNATURE]; }
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

		#region IPaymentGatewayProvider Members

		public TransactionResult SubmitPaymentTransaction(CheckoutDetails details)
		{
			//init result structure
			TransactionResult ret = new TransactionResult();

			//set up Request
			//instantiate DoDirectPaymentRequestType and RequestDetails objects
			DoDirectPaymentRequestType request = new DoDirectPaymentRequestType();
			request.Version = PROCESSOR_VERSION;
			DoDirectPaymentRequestDetailsType requestDetails = new DoDirectPaymentRequestDetailsType();
			request.DoDirectPaymentRequestDetails = requestDetails;

			//set payment action
			requestDetails.PaymentAction = PaymentActionCodeType.Sale;

			//set IP
			//requestDetails.IPAddress = Request.UserHostAddress;
			requestDetails.IPAddress = details[CheckoutKeys.IPAddress];

			//set CreditCard info
			CreditCardDetailsType creditCardDetails = new CreditCardDetailsType();
			requestDetails.CreditCard = creditCardDetails;
			creditCardDetails.CreditCardNumber = details[CheckoutKeys.CardNumber];
			creditCardDetails.CreditCardType = (CreditCardTypeType)Enum.Parse(typeof(CreditCardTypeType), details[CheckoutKeys.CardType], true);
			creditCardDetails.CVV2 = details[CheckoutKeys.VerificationCode];
			creditCardDetails.ExpMonth = Int32.Parse(details[CheckoutKeys.ExpireMonth]);
			creditCardDetails.ExpYear = Int32.Parse(details[CheckoutKeys.ExpireYear]);
			// Switch/Solo
			if (creditCardDetails.CreditCardType == CreditCardTypeType.Solo ||
				creditCardDetails.CreditCardType == CreditCardTypeType.Switch)
			{
				creditCardDetails.StartMonth = Int32.Parse(details[CheckoutKeys.StartMonth]);
				creditCardDetails.StartYear = Int32.Parse(details[CheckoutKeys.StartYear]);
				creditCardDetails.IssueNumber = details[CheckoutKeys.IssueNumber];
			}

			//set billing address
			PayerInfoType cardOwner = new PayerInfoType();
			creditCardDetails.CardOwner = cardOwner;
			cardOwner.PayerName = new PersonNameType();
			cardOwner.PayerName.FirstName = details[CheckoutKeys.FirstName];
			cardOwner.PayerName.LastName = details[CheckoutKeys.LastName];

			cardOwner.Address = new AddressType();
			cardOwner.Address.Street1 = details[CheckoutKeys.Address];
			//??? cardOwner.Address.Street2 = "";
			cardOwner.Address.CityName = details[CheckoutKeys.City];
			cardOwner.Address.StateOrProvince = details[CheckoutKeys.State];
			cardOwner.Address.PostalCode = details[CheckoutKeys.Zip];
			cardOwner.Address.CountrySpecified = true;
			cardOwner.Address.Country = (CountryCodeType)Enum.Parse(typeof(CountryCodeType), details[CheckoutKeys.Country], true);

			//set payment Details
			PaymentDetailsType paymentDetails = new PaymentDetailsType();
			requestDetails.PaymentDetails = paymentDetails;
			paymentDetails.OrderTotal = new BasicAmountType();
			//TODO: Add currency support
			paymentDetails.OrderTotal.currencyID = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), details[CheckoutKeys.Currency]);
			//paymentDetails.OrderTotal.currencyID = CurrencyCodeType.USD;
			//No currency symbol. Decimal separator must be a period (.), and the thousands separator must be a comma (,)
			paymentDetails.OrderTotal.Value = details[CheckoutKeys.Amount];

			DoDirectPaymentReq paymentRequest = new DoDirectPaymentReq();
			paymentRequest.DoDirectPaymentRequest = request;

			//FINISH set up req
			//setup request Header, API credentials
			PayPalAPIAASoapBinding paypalInterface = new PayPalAPIAASoapBinding();
			UserIdPasswordType user = new UserIdPasswordType();

			//set api credentials - username, password, signature
			user.Username = Username;
			user.Password = Password;
			user.Signature = Signature;
			// setup service url
			paypalInterface.Url = ServiceUrl;
			paypalInterface.RequesterCredentials = new CustomSecurityHeaderType();
			paypalInterface.RequesterCredentials.Credentials = user;

			//make call return response
			DoDirectPaymentResponseType paymentResponse = new DoDirectPaymentResponseType();
			paymentResponse = paypalInterface.DoDirectPayment(paymentRequest);
			//write response xml to the ret object
			ret.RawResponse = SerializeObject(paymentResponse);
			
			switch (paymentResponse.Ack)
			{
				case AckCodeType.Success:
				case AckCodeType.SuccessWithWarning:
					ret.Succeed = true;
					ret.TransactionId = paymentResponse.TransactionID;
					ret.TransactionStatus = TransactionStatus.Approved;
					break;
				default: // show errors if Ack is NOT Success
					ret.Succeed = false;
					ret.TransactionStatus = TransactionStatus.Declined;
					if (paymentResponse.Errors != null &&
						paymentResponse.Errors.Length > 0)
					{
						ret.StatusCode = PayPalProKeys.ErrorPrefix + paymentResponse.Errors[0].ErrorCode;
					}
					break;
			}
			return ret;
		}
		#endregion

		private string UTF8ByteArrayToString(byte[] characters)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			string ret = encoding.GetString(characters);
			return ret;
		}

		private string SerializeObject(object obj)
		{
			string ret = null; 
			if (obj == null)
			{
				return ret;
			}

			XmlTextWriter xmlTextWriter = null;
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
				xmlTextWriter = new XmlTextWriter(new MemoryStream(), Encoding.UTF8);
				xmlSerializer.Serialize(xmlTextWriter, obj);
				MemoryStream memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
				ret = UTF8ByteArrayToString(memoryStream.ToArray());
				return ret;
			}
			catch (Exception)
			{
				//write to log
				return ret;
			}
			finally
			{
				if (xmlTextWriter != null)
				{
					xmlTextWriter.Close();
				}
			}
		}
	}
}

