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
using System.Text;

namespace SolidCP.Ecommerce.EnterpriseServer
{
	/// <summary>
	/// Authorize.NET Payment Provider keys set
	/// </summary>
	public class AuthorizeNetKeys
	{
		/// <summary>
		/// 3.1
		/// </summary>
		public const string Version = "x_version";
		/// <summary>
		/// True
		/// </summary>
		public const string DelimData = "x_delim_data";
		/// <summary>
		/// False
		/// </summary>
		public const string RelayResponse = "x_relay_response";
		/// <summary>
		/// API login ID for the payment gateway account
		/// </summary>
		public const string Account = "x_login";
		/// <summary>
		/// Transaction key obtained from the Merchant Interface
		/// </summary>
		public const string TransactionKey = "x_tran_key";
		/// <summary>
		/// Amount of purchase inclusive of tax
		/// </summary>
		public const string Amount = "x_amount";
		/// <summary>
		/// Customer's card number
		/// </summary>
		public const string CardNumber = "x_card_num";
		/// <summary>
		/// Customer's card expiration date
		/// </summary>
		public const string ExpirationDate = "x_exp_date";
		/// <summary>
		/// Type of transaction (AUTH_CAPTURE, AUTH_ONLY, CAPTURE_ONLY, CREDIT, VOID, PRIOR_AUTH_CAPTURE)
		/// </summary>
		public const string TransactType = "x_type";
		/// <summary>
		/// 
		/// </summary>
		public const string DemoMode = "x_test_request";
		public const string DelimiterChar = "x_delim_char";
		public const string EncapsulationChar = "x_encap_char";
		public const string DuplicateWindow = "x_duplicate_window";

		public const string FirstName = "x_first_name";
		public const string LastName = "x_last_name";
		public const string Company = "x_company";
		public const string Address = "x_address";
		public const string City = "x_city";
		public const string State = "x_state";
		public const string Zip = "x_zip";
		public const string Country = "x_country";
		public const string Phone = "x_phone";
		public const string Fax = "x_fax";
		public const string CustomerId = "x_cust_id";
		public const string IPAddress = "x_customer_ip";
		public const string CustomerTax = "x_customer_tax_id";
		public const string CustomerEmail = "x_email";
		public const string SendConfirmation = "x_email_customer";
		public const string MerchantEmail = "x_merchant_email";
		public const string InvoiceNumber = "x_invoice_num";
		public const string TransDescription = "x_description";

		public const string CurrencyCode = "x_currency_code";
		public const string PaymentMethod = "x_method";
		public const string RecurringBilling = "x_recurring_billing";
		public const string VerificationCode = "x_card_code";
		public const string AuthorizationCode = "x_auth_code";
		public const string AuthenticationIndicator = "x_authentication_indicator";
		public const string PurchaseOrder = "x_po_num";
		public const string Tax = "x_tax";

		public const string FpHash = "x_fp_hash";
		public const string FpSequence = "x_fp_sequence";
		public const string FpTimestamp = "x_fp_timestamp";

		public const string RelayUrl = "x_relay_url";

		public const string TransactId = "x_trans_id";
		public const string AuthCode = "x_auth_code";

		public const string MD5HashValue = "MD5HashValue";

		public const string ShipToFirstName = "x_ship_to_first_name";
		public const string ShipToLastName = "x_ship_to_last_name";
		public const string ShipToCompany = "x_ship_to_company";
		public const string ShipToAddress = "x_ship_to_address";
		public const string ShipToCity = "x_ship_to_city";
		public const string ShipToState = "x_ship_to_state";
		public const string ShipToZip = "x_ship_to_zip";
		public const string ShipToCountry = "x_ship_to_country";

		public const string ErrorPrefix = "AuthorizeNet.";

		private AuthorizeNetKeys()
		{
		}
	}
}

