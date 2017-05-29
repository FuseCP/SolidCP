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
	public class OfflinePayment : SystemPluginBase, IPaymentGatewayProvider
	{
        public const string DEFAULT_TRANSACTION_FORMAT = "yyyyMMdd-[INVOICE_ID]";
		public const string ErrorPrefix = "OfflinePayment.";
		/// <summary>
		/// Gets payment prefix
		/// </summary>
        public string TransactionNumberFormat
        {
            get
            {
                string formatStr = PluginSettings[OffPaymentSettings.TRANSACTION_NUMBER_FORMAT];
                //
                if (String.IsNullOrEmpty(formatStr))
                    formatStr = DEFAULT_TRANSACTION_FORMAT;
                //
                return formatStr;
            }
        }

		public bool AutoApprove
		{
			get { return Convert.ToBoolean(PluginSettings[OffPaymentSettings.AUTO_APPROVE]); }
		}

		public OfflinePayment()
		{
		}

		#region IPaymentGatewayProvider Members

		public TransactionResult SubmitPaymentTransaction(CheckoutDetails details)
		{
			TransactionResult result = new TransactionResult();

            // 1. Process date and time variables
            string transactionNumber = DateTime.Now.ToString(TransactionNumberFormat);
            // 2. Process E-Commerce variables
            transactionNumber = transactionNumber.Replace("[INVOICE_ID]", details[CheckoutKeys.InvoiceNumber]);

			// transaction is ok
			result.Succeed = true;
			// set transation id
			result.TransactionId = transactionNumber;
			// status code is empty
			result.StatusCode = String.Empty;
			// no response available
			result.RawResponse = "No response available";

			// check payment approval setting
			if (AutoApprove)
				result.TransactionStatus = TransactionStatus.Approved;
			else
				result.TransactionStatus = TransactionStatus.Pending;

			// return result
			return result;
		}

		#endregion
	}
}
