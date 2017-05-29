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
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Ecommerce.EnterpriseServer
{
	public enum AIMField
	{
		ResponseCode = 0,
		ResponseSubCode = 1,
		ResponseReasonCode = 2,
		ResponseReasonText = 3,
		ApprovalCode = 4,
		AvsResultCode = 5,
		TransactionId = 6,
		RespInvoiceNum = 7,
		Description = 8,
		RespAmount = 9,
		RespPaymentMethod = 10,
		TransactionType = 11,
		RespCustomerId = 12,
		RespFirstName = 13,
		RespLastName = 14,
		RespCompany = 15,
		BillingAddress = 16,
		RespCity = 17,
		RespState = 18,
		Zip = 19,
		Country = 20,
		Phone = 21,
		Fax = 22,
		Email = 23,
		ShipToFirstName = 24,
		ShipToLastName = 25,
		ShipToCompany = 26,
		ShipToAddress = 27,
		ShipToCity = 28,
		ShipToState = 29,
		ShipToZip = 30,
		ShipToCountry = 31,
		TaxAmount = 32,
		DutyAmount = 33,
		FreightAmount = 34,
		TaxExemptFlag = 35,
		PurchaseOrderNumber = 36,
		ResponseSignature = 37,
		CardVerificationCode = 38
	};

	public class AIMResponse
	{
		private string[] _aimData;
		private string _rawResponse;
		private int _aimLength;
		private char _delimChar = '|';

		public string RawResponse
		{
			get { return _rawResponse; }
		}

		public AIMResponse(Stream stream, char delimiter)
		{
			_delimChar = delimiter;
			StreamReader sr = new StreamReader(stream);
			string response = sr.ReadToEnd();
			sr.Close();

			Initialize(response);
		}

		public AIMResponse(string response)
		{
			Initialize(response);
		}

		private void Initialize(string response)
		{
			if (string.IsNullOrEmpty(response))
				throw new Exception("Response data is empty.");

			_aimData = response.Split(_delimChar);
			_aimLength = _aimData.Length;

			if (_aimData.Length == 0 && response.Length > 0)
				throw new Exception("Invalid response data format.");

			_rawResponse = response;
		}

		public string this[AIMField field]
		{
			get
			{
				int index = (int)field;

				if (index < _aimLength)
					return _aimData[index];

				return null;
			}
		}
	}

}
