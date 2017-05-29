// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
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

﻿using System;
using System.Collections.Generic;

namespace SolidCP.Providers.HostedSolution
{
    public abstract class BaseReport<T> where T : BaseStatistics
	{
		private List<T> items = new List<T>();

		public List<T> Items
		{
			get { return items; }
		}

        public abstract string ToCSV();
		
		/// <summary>
		/// Converts source string into CSV string.
		/// </summary>
		/// <param name="source">Source string.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(string source)
		{
			string ret = source;
			if (!string.IsNullOrEmpty(source))
			{
				if (source.IndexOf(',') >= 0 || source.IndexOf('"') >= 0)
					ret = "\"" + source.Replace("\"", "\"\"") + "\"";
			}
			return ret;
		}

		/// <summary>
		/// Converts DateTime string into CSV string.
		/// </summary>
		/// <param name="date">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(DateTime date)
		{
			string ret = string.Empty;
			if (date != DateTime.MinValue)
			{
				ret = date.ToString("G");
			}
			return ToCsvString(ret);
		}

		/// <summary>
		/// Converts long value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(long val)
		{
			return ToCsvString(val.ToString());
		}

		/// <summary>
		/// Converts int value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(int val)
		{
			return ToCsvString(val.ToString());
		}

		/// <summary>
		/// Converts unlimited int value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string UnlimitedToCsvString(int val)
		{
			string ret = string.Empty;
			ret = (val == -1) ? "Unlimited" : val.ToString();
			return ToCsvString(ret);
		}

		/// <summary>
		/// Converts unlimited long value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string UnlimitedToCsvString(long val)
		{
			string ret = string.Empty;
			ret = (val == -1) ? "Unlimited" : val.ToString();
			return ToCsvString(ret);
		}

		/// <summary>
		/// Converts unlimited long value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string UnlimitedToCsvString(double val)
		{
			string ret = string.Empty;
			ret = (val == -1d) ? ToCsvString("Unlimited") : ToCsvString(val);
			return ret;
		}

		/// <summary>
		/// Converts double value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(double val)
		{
			return ToCsvString(val.ToString("F"));
		}

		/// <summary>
		/// Converts boolean value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(bool val, string trueValue, string falseValue)
		{
			return ToCsvString(val ? trueValue : falseValue);
		}

		/// <summary>
		/// Converts boolean value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(bool val)
		{
			return ToCsvString(val, "Enabled", "Disabled");
		}

		/// <summary>
		/// Converts value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(ExchangeAccountType val)
		{
			string ret = string.Empty;
			switch (val)
			{
				case ExchangeAccountType.Contact:
					ret = "Contact";
					break;
				case ExchangeAccountType.DistributionList:
					ret = "Distribution List";
					break;
				case ExchangeAccountType.Equipment:
					ret = "Equipment Mailbox";
					break;
				case ExchangeAccountType.Mailbox:
					ret = "User Mailbox";
					break;
				case ExchangeAccountType.PublicFolder:
					ret = "Public Folder";
					break;
				case ExchangeAccountType.Room:
					ret = "Room Mailbox";
					break;
				case ExchangeAccountType.User:
					ret = "User";
					break;
				default:
					ret = "Undefined";
					break;

			}
			return ToCsvString(ret);
		}

	}
}
