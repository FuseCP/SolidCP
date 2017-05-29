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
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SolidCP.Ecommerce.EnterpriseServer
{
    public class TCOProvider : SystemPluginBase, IInteractivePaymentGatewayProvider
	{
		#region Error Messages

		public const string CURRENCY_NOT_MATCH_MSG = "Invoice currency doesn't conform with the 2CheckOut plug-in settings. Please check that your storefront base currency matches with the 2CO plug-in currency";
		public const string KEY_VALIDATION_FAILED_MSG = "Key validation failed. Original response has been either corrupted or modified.";
		public const string CC_PROCESSED_ERROR_MSG = "2CheckOut is unable to process your credit card";

		#endregion

		public const string PAYMENT_ROUTINE = "https://www.2checkout.com/2co/buyer/purchase";
		public const string ErrorPrefix = "2Checkout.";
		public const string CREDIT_CARD_PROCESSED = "credit_card_processed";
		public const string DEMO_MODE = "demo";
		public const string KEY = "key";
		public const string OUTSIDE_US_CA = "Outside US and Canada";

		public override string[] SecureSettings
		{
			get
			{
				return new string[] { ToCheckoutSettings.SECRET_WORD };
			}
		}

		/// <summary>
		/// Gets whether 2CO plug-in running in demo mode
		/// </summary>
		public bool LiveMode
		{
			get
			{
				return Convert.ToBoolean(PluginSettings[ToCheckoutSettings.LIVE_MODE]);
			}
		}
		/// <summary>
		/// Gets whether 2CO plug-in should disable quantity field
		/// </summary>
		public bool FixedCart
		{
			get
			{
				return Convert.ToBoolean(PluginSettings[ToCheckoutSettings.FIXED_CART]);
			}
		}
		/// <summary>
		/// Gets plug-in secret word
		/// </summary>
		public string SecretWord
		{
			get { return PluginSettings[ToCheckoutSettings.SECRET_WORD]; }
		}
		/// <summary>
		/// Gets 2CO account sid
		/// </summary>
		public string AccountSID
		{
			get { return PluginSettings[ToCheckoutSettings.ACCOUNT_SID]; }
		}
		/// <summary>
		/// Gets 2CO currency
		/// </summary>
		public string TCO_Currency
		{
			get { return PluginSettings[ToCheckoutSettings.CURRENCY]; }
		}
		/// <summary>
		/// Gets 2CO continue shopping button url
		/// </summary>
		public string ContinueShoppingUrl
		{
			get { return PluginSettings[ToCheckoutSettings.CONTINUE_SHOPPING_URL]; }
		}

		public TCOProvider()
		{
		}

		#region IPaymentGatewayProvider Members

        public CheckoutFormParams GetCheckoutFormParams(FormParameters inputParams, InvoiceItem[] invoiceLines)
        {
			// check invoice currency against 2CO settings
			if (!CI_ValuesEqual(TCO_Currency, inputParams[FormParameters.CURRENCY]))
				throw new Exception(CURRENCY_NOT_MATCH_MSG);

			// fill checkout params
			CheckoutFormParams outputParams = new CheckoutFormParams();
			// sets serviced props
			outputParams.Action = PAYMENT_ROUTINE;
			outputParams.Method = CheckoutFormParams.POST_METHOD;
			// copy target_site variable
			outputParams["target_site"] = inputParams["target_site"];
			// copy invoice amount
			outputParams["sid"] = AccountSID;
            // copy contract number
            outputParams["contract_id"] = inputParams[FormParameters.CONTRACT];
			// copy invoice number
			outputParams["merchant_order_id"] = inputParams[FormParameters.INVOICE];
			// copy invoice currency
			outputParams["tco_currency"] = inputParams[FormParameters.CURRENCY];
			// copy continue shopping button url
			// outputParams["return_url"] = ContinueShoppingUrl + "&ContractId=" + inputParams[FormParameters.CONTRACT];
			// copy fixed cart option
			if (FixedCart)
				outputParams["fixed"] = "Y";
			// copy pay method (credit card)
			outputParams["pay_method"] = "CC";
			// copy live mode flag
			if (!LiveMode)
				outputParams["demo"] = "Y";
			// copy card holder name
			outputParams["card_holder_name"] = inputParams[FormParameters.FIRST_NAME] + " " + inputParams[FormParameters.LAST_NAME];
			// copy email
			outputParams["email"] = inputParams[FormParameters.EMAIL];
			// copy street address
			outputParams["street_address"] = inputParams[FormParameters.ADDRESS];
			// copy country
			outputParams["country"] = inputParams[FormParameters.COUNTRY];
			// copy city
			outputParams["city"] = inputParams[FormParameters.CITY];
			// copy state & phone
			if (!CI_ValuesEqual(inputParams[FormParameters.COUNTRY], "US") &&
				!CI_ValuesEqual(inputParams[FormParameters.COUNTRY], "CA"))
			{
				// state is outside US & CA
				outputParams["state"] = OUTSIDE_US_CA;
				// copy outside US phone as is 
				outputParams["phone"] = inputParams[FormParameters.PHONE];
			}
			else
			{
				// copy state
				outputParams["state"] = inputParams[FormParameters.STATE];
				// convert phone to US format
				outputParams["phone"] = ConvertPhone2US(inputParams[FormParameters.PHONE]);
			}
			// copy zip
			outputParams["zip"] = inputParams[FormParameters.ZIP];
			// copy invoice amount
			outputParams["total"] = inputParams[FormParameters.AMOUNT];
			// copy invoice number
			outputParams["cart_order_id"] = inputParams[FormParameters.INVOICE];

			return outputParams;
        }

		public TransactionResult SubmitPaymentTransaction(CheckoutDetails details)
		{
			TransactionResult result = new TransactionResult();
			// build raw response for 2CO
			string[] keys = details.GetAllKeys();
			List<string> bunch = new List<string>();
			// copy checkout details
			foreach (string key in keys)
			{
				bunch.Add(String.Concat(key, "=", details[key]));
			}
			// build raw 2CO response
			result.RawResponse = String.Join("|", bunch.ToArray());
			// recognize credit card status
            switch(details[CREDIT_CARD_PROCESSED])
            {
                case "Y":
                    result.TransactionStatus = TransactionStatus.Approved;
                    break;
                case "K":
                    result.TransactionStatus = TransactionStatus.Pending;
                    break;
                default:
                    throw new Exception(CC_PROCESSED_ERROR_MSG);
            }
			// read order number
			string order_number = details["order_number"];
			// check demo mode: set order number to 1
			// according to 2Checkout documentation for demo transactions
			
			bool valid = false;
			// validate TCO key
			if (LiveMode) // do live validation
				valid = ValidateKey(SecretWord, AccountSID, order_number, details[CheckoutKeys.Amount], details[KEY]);
			else // do demo validation
				valid = ValidateKey(SecretWord, AccountSID, "1", details[CheckoutKeys.Amount], details[KEY]);

			// key validation failed
			if (!valid)
				throw new ArgumentException(KEY_VALIDATION_FAILED_MSG);
			// we are succeed copy order number
			result.TransactionId = order_number;
			//
			result.Succeed = true;
			// return result
			return result;
		}

		#endregion

		private bool ValidateKey(string secretWord, string vendorNumber, string tcoOrderNumber, string tcoTotal, string tcoKey)
		{
			string rawString = String.Concat(secretWord, vendorNumber, tcoOrderNumber, tcoTotal);

			System.Text.Encoding encoding = System.Text.Encoding.ASCII;

			byte[] bufferIn = encoding.GetBytes(rawString);
			MD5CryptoServiceProvider cryptoProv = new MD5CryptoServiceProvider();
			byte[] bufferOut = cryptoProv.ComputeHash(bufferIn);

			string hashString = String.Empty;

			for (int i = 0; i < bufferOut.Length; i++)
			{
				hashString += Convert.ToString(bufferOut[i], 16).PadLeft(2, '0');
			}

			hashString = hashString.PadLeft(32, '0').ToUpper();

			return (String.Compare(hashString, tcoKey, true) == 0);
		}

		private string ConvertPhone2US(string phone)
		{
			string result = "";
			// loop for each digit in phone
			foreach (char c in phone.ToCharArray())
			{
				// exit from loop if phone length is exceeded
				if (result.Length == 12)
					break;
				// check character
				if (Char.IsDigit(c))
				{
					// append delimiter
					if (result.Length == 3 || result.Length == 6)
						result += "-";
					// build phone number
					result += c.ToString();
				}
			}
			// return result formatted string
			return result;
		}

		private bool CI_ValuesEqual(string strA, string strB)
		{
			return String.Equals(strA, strB, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
