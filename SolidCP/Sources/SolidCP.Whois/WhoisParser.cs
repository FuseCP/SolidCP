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
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SolidCP.Ecommerce.EnterpriseServer
{
	/* Method template to create a new TLD match
	 * WHOIS server response is read line-by-line so this template works almost for all types of response
		private static void Parse_REGISTRAR_ID_GOES_HERE(WhoisResult whoisResult, StringReader whoisResponse)
		{
			string sLine = "";
			List<string> nameServers = new List<string>();
			StringBuilder errorMessage = new StringBuilder();
			bool throwError = true;

			while (sLine != null)
			{
				// advance to the next line
				sLine = whoisResponse.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record not found
				if (Regex.IsMatch(sLine, "RECORD NOT FOUND regex to match goes here..."))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Record found
				if (Regex.IsMatch(sLine, @"DOMAIN NAME MATCH (for example - google.com) regex goes here..."))
				{
					// response is ok
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 3. Copy nameservers
				if (sLine.Equals("DNS SERVERS match goes here..."))
				{
					// advance to the next line
					sLine = whoisResponse.ReadLine();
					//
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for nameserver match
						Match nsMatch = nameServersRegex.Match(sLine);
						// add ns record
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = whoisResponse.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw an error if error message is not empty
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}
	 */

	internal class WhoisParser
	{
		public const string UNKNOWN_FORMAT_FAILURE = "Couldn't find any formatters for the specified WHOIS server {0}.";
		public const string WHOIS_LIMIT_EXCEED_FAILURE = "You've exceeded WHOIS queries limit per minute. Please try later.";
		public const string WHOIS_EMPTY_QUERY_FAILURE = "WHOIS query may not be an empty string.";

		// regular expression for nameserver string
		private static readonly Regex nameServersRegex = 
			new Regex(@"([a-zA-Z0-9\-\.]+\.[a-zA-Z0-9\-]+\.[a-zA-Z\-]+)");

		static WhoisParser()
		{
		}

		public static WhoisResult Parse(string domain, string whoisServer, StringReader whoisResponse)
		{
			WhoisResult result = new WhoisResult();
			result.Domain = domain;

			string whoisFormat = (string)WhoisSettings.Parsers[whoisServer];

			switch (whoisFormat)
			{
				case WhoisSettings.UANIC:
					Parse_UANIC(result, whoisResponse);
					break;
				case WhoisSettings.INTERNIC:
					Parse_INTERNIC(result, whoisResponse);
					break;
				case WhoisSettings.AFFILIAS_LTD:
				case WhoisSettings.PIR:
                case WhoisSettings.mTLD:
					Parse_PIR(result, whoisResponse);
					break;
				case WhoisSettings.EDUCAUSE:
					Parse_EDUCAUSE(result, whoisResponse);
					break;
				case WhoisSettings.NOMINET:
					Parse_NOMINET(result, whoisResponse);
					break;
				case WhoisSettings.AUREGISTRY:
					Parse_AUREGISTRY(result, whoisResponse);
					break;
				case WhoisSettings.EURID:
					Parse_EURID(result, whoisResponse);
					break;
				case WhoisSettings.ROMANIAN:
					Parse_ROMANIAN(result, whoisResponse);
					break;
				case WhoisSettings.SWITCH:
					Parse_SWITCH(result, whoisResponse);
					break;
				case WhoisSettings.NEULEVEL:
				case WhoisSettings.NEUSTAR:
					Parse_NEUSTAR(result, whoisResponse);
					break;
				case WhoisSettings.SIDN:
					Parse_SIDN(result, whoisResponse);
					break;
                case WhoisSettings.AFNIC:
                    Parse_AFNIC(result, whoisResponse);
                    break;
                case WhoisSettings.TIERED_ACCESS:
                    Parse_TIERED_ACCESS(result, whoisResponse);
                    break;
				case WhoisSettings.INTERNET_NZ:
					Parse_INTERNET_NZ(result, whoisResponse);
					break;
				default:
					throw new Exception(String.Format(UNKNOWN_FORMAT_FAILURE, whoisServer));
			}

			return result;
		}

		private static void Parse_UANIC(WhoisResult whoisResult, StringReader whoisResponse)
		{
			string sLine = "";
			List<string> nameServers = new List<string>();
			StringBuilder errorMessage = new StringBuilder();
			// According to the research it seems 
			// UA WHOIS servers do not throw any errors, so do we.
			while (sLine != null)
			{
				// advance to the next line
				sLine = whoisResponse.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record found
				if (Regex.IsMatch(sLine, @"domain\: .*ua"))
				{
					// response is ok
					whoisResult.RecordFound = true;
					continue;
				}
				// 2. Copy nameservers
				if (sLine.StartsWith("nserver"))
				{
					//
					while (sLine.StartsWith("nserver") 
						&& nameServersRegex.IsMatch(sLine))
					{
						// lookup for nameserver match
						Match nsMatch = nameServersRegex.Match(sLine);
						// add ns record
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = whoisResponse.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
				}
			}
		}

		private static void Parse_INTERNET_NZ(WhoisResult whoisResult, StringReader whoisResponse)
		{
			string sLine = "";
			List<string> nameServers = new List<string>();
			StringBuilder errorMessage = new StringBuilder();
			bool throwError = true;

			while (sLine != null)
			{
				// advance to the next line
				sLine = whoisResponse.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record found
				if (Regex.IsMatch(sLine, @"domain_name\: .*nz"))
				{
					// response is ok
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 2. Match query status
				if (Regex.IsMatch(sLine, @"query_status\:"))
				{
					// Available
					if (sLine.Contains("220"))
					{
						whoisResult.RecordFound = false;
						return; // EXIT
					}
					// Active
					if (sLine.Contains("200"))
						continue;
					else
						throwError = true;
				}
				
				// 3. Copy nameservers
				if (sLine.StartsWith("ns_name_"))
				{
					//
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for nameserver match
						Match nsMatch = nameServersRegex.Match(sLine);
						// add ns record
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = whoisResponse.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw an error if error message is not empty
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

		private static void Parse_NUDOMAINLTD(WhoisResult whoisResult, StringReader whoisResponse)
		{
			string sLine = "";
			List<string> nameServers = new List<string>();
			StringBuilder errorMessage = new StringBuilder();
			bool throwError = true;

			while (sLine != null)
			{
				// advance to the next line
				sLine = whoisResponse.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record not found
				if (Regex.IsMatch(sLine, "NO MATCH for domain \\\".*nu\\\" \\(ASCII\\)\\:"))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Record found
				if (Regex.IsMatch(sLine, @"Domain Name \(ASCII\)\: .*nu"))
				{
					// response is ok
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 3. Copy nameservers
				if (sLine.Equals("Domain servers in listed order:"))
				{
					// advance to the next line
					sLine = whoisResponse.ReadLine();
					//
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for nameserver match
						Match nsMatch = nameServersRegex.Match(sLine);
						// add ns record
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = whoisResponse.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw an error if error message is not empty
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

        private static void Parse_TIERED_ACCESS(WhoisResult whoisResult, StringReader sr)
        {
            string sLine = "";
            List<string> nameServers = new List<string>();
            StringBuilder errorMessage = new StringBuilder();
            bool throwError = true;

            while (sLine != null)
            {
                // advance to the next line
                sLine = sr.ReadLine();
                // skip empty lines
                if (String.IsNullOrEmpty(sLine))
                    continue;
                // trim whitespaces
                sLine = sLine.Trim();
                // 1. Record not found
                if (sLine.Equals("No match."))
                {
                    whoisResult.RecordFound = false;
                    return; // EXIT
                }
                // 2. Record found
                if (Regex.IsMatch(sLine, "Not available for.*registration"))
                {
                    // response is ok
                    throwError = false;
                    whoisResult.RecordFound = true;
                    continue;
                }
                // 3. Copy nameservers
                if (sLine.StartsWith("Name Server:"))
                {
                    while (nameServersRegex.IsMatch(sLine))
                    {
                        // lookup for nameserver match
                        Match nsMatch = nameServersRegex.Match(sLine);
                        // add ns record
                        nameServers.Add(nsMatch.Value.ToLower());
                        // advance to the next line
                        sLine = sr.ReadLine();
                    }
                    // copy result
                    whoisResult.NameServers = nameServers.ToArray();
                }
                // 4. Response contains errors
                errorMessage.AppendLine(sLine);
            }
            // throw an error if error message is not empty
            if (throwError)
                throw new WhoisException(errorMessage.ToString());
        }

        private static void Parse_AFNIC(WhoisResult whoisResult, StringReader sr)
        {
            string sLine = "";
            List<string> nameServers = new List<string>();
            StringBuilder errorMessage = new StringBuilder();
            bool throwError = true;

            while (sLine != null)
            {
                // advance to the next line
                sLine = sr.ReadLine();
                // skip empty lines
                if (String.IsNullOrEmpty(sLine))
                    continue;
                // trim whitespaces
                sLine = sLine.Trim();
                // 1. Record not found
                if (sLine.Equals("%% No entries found in the AFNIC Database."))
                {
                    whoisResult.RecordFound = false;
                    return; // EXIT
                }
                // 2. Record found
                if (sLine.StartsWith("domain:"))
                {
                    // response is ok
                    throwError = false;
                    whoisResult.RecordFound = true;
                    continue;
                }
                // 3. Copy nameservers
                if (sLine.StartsWith("nserver:"))
                {
                    while (nameServersRegex.IsMatch(sLine))
                    {
                        // lookup for nameserver match
                        Match nsMatch = nameServersRegex.Match(sLine);
                        // add ns record
                        nameServers.Add(nsMatch.Value.ToLower());
                        // advance to the next line
                        sLine = sr.ReadLine();
                    }
                    // copy result
                    whoisResult.NameServers = nameServers.ToArray();
                }
                // 4. Response contains errors
                errorMessage.AppendLine(sLine);
            }
            // throw an error if error message is not empty
            if (throwError)
                throw new WhoisException(errorMessage.ToString());
        }

		private static void Parse_SIDN(WhoisResult whoisResult, StringReader sr)
		{
			string sLine = "";
			List<string> nameServers = new List<string>();
			StringBuilder errorMessage = new StringBuilder();
			bool throwError = true;

			while (sLine != null)
			{
				// advance to the next line
				sLine = sr.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record not found
				if (sLine.Equals(whoisResult.Domain + " is free"))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Record found
				if (sLine.StartsWith("Domain name:"))
				{
					// response is ok
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 3. Copy nameservers
				if (sLine.StartsWith("Domain nameservers:"))
				{
					// advance to the next line
					sLine = sr.ReadLine();
					// 
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for nameserver match
						Match nsMatch = nameServersRegex.Match(sLine);
						// add ns record
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = sr.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw an error if error message is not empty
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

		private static void Parse_INTERNIC(WhoisResult whoisResult, StringReader sr)
		{
			string sLine = "";
			List<string> nameServers = new List<string>();
			StringBuilder errorMessage = new StringBuilder();
			bool throwError = true;

			while (sLine != null)
			{
				// advance to the next line
				sLine = sr.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record not found
				if (sLine.StartsWith("No match for "))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Record found
				if (sLine.StartsWith("Domain Name:"))
				{
					// response is ok
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 3. Copy nameservers
				if (sLine.StartsWith("Name Server:"))
				{
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for nameserver match
						Match nsMatch = nameServersRegex.Match(sLine);
						// add ns record
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = sr.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw an error if error message is not empty
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

		private static void Parse_PIR(WhoisResult whoisResult, StringReader sr)
		{
			string sLine = "";
			List<string> nameServers = new List<string>();
			StringBuilder errorMessage = new StringBuilder();
			bool throwError = true;

			while (sLine != null)
			{
				// advance to the next line
				sLine = sr.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record not found
				if (sLine.StartsWith("NOT FOUND"))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Record found
				if (sLine.StartsWith("Domain Name:"))
				{
					// response is ok
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 3. Copy nameservers
				if (sLine.StartsWith("Name Server:"))
				{
					while (sLine != null)
					{
						// exit when we finish with nameservers
						if (!sLine.StartsWith("Name Server:")) break;
						// lookup for nameserver match
						Match nsMatch = nameServersRegex.Match(sLine);
						if (nsMatch.Success)
							nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = sr.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
					return;
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw an error if error message is not empty
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

		private static void Parse_EDUCAUSE(WhoisResult whoisResult, StringReader sr)
		{
			string sLine = "";
			List<string> nameServers = new List<string>();
			StringBuilder errorMessage = new StringBuilder();
			bool throwError = true;

			while (sLine != null)
			{
				// read line
				sLine = sr.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record not found
				if (sLine.StartsWith("No Match"))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Domain record found
				if (sLine.StartsWith("Domain Name:"))
				{
					// response is ok
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 3. Found name servers records
				if (sLine.StartsWith("Name Servers:"))
				{
					// advance to the next line
					sLine = sr.ReadLine();
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for ns match
						Match nsMatch = nameServersRegex.Match(sLine);
						// add name server record
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = sr.ReadLine();
					}
					// copy found ns records
					whoisResult.NameServers = nameServers.ToArray();
					return; // EXIT
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw an error if any
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

		private static void Parse_NOMINET(WhoisResult whoisResult, StringReader sr)
		{
			string sLine = "";
			StringBuilder errorMessage = new StringBuilder();
			List<string> nameServers = new List<string>();
			bool throwError = true;

			// read 
			while (sLine != null)
			{
				// advance to the next line
				sLine = sr.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record not found
				if (sLine.StartsWith("No match for"))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Check record found
				if (sLine.StartsWith("Domain name:"))
				{
					// response is ok
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 3. Record found lookup for nameservers
				if (sLine.StartsWith("Name servers:"))
				{
					// advance to the next line
					sLine = sr.ReadLine();
					// loop for name servers records
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for match
						Match nsMatch = nameServersRegex.Match(sLine);
						// push nameserver to the list
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = sr.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
					return; // EXIT
				}
				// 4. Collect response lines
				errorMessage.AppendLine(sLine);
			}
			// throw an whois error
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

		private static void Parse_AUREGISTRY(WhoisResult whoisResult, StringReader sr)
		{
			string sLine = "";
			StringBuilder errorMessage = new StringBuilder();
			List<string> nameServers = new List<string>();
			bool throwError = true;

			// read 
			while (sLine != null)
			{
				// advance to the next line
				sLine = sr.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record not found
				if (sLine.StartsWith("No Data Found"))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Check record found
				if (sLine.StartsWith("Domain Name:"))
				{
					// response is ok
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 3. Record found lookup for nameservers
				if (sLine.StartsWith("Name Server:"))
				{
					// loop for name servers records
					while (sLine != null)
					{
						// lookup for match
						Match nsMatch = nameServersRegex.Match(sLine);
						// check lookup status and push nameserver to the list
						if (nsMatch.Success)
							nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = sr.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
					return; // EXIT
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw whois error
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

		private static void Parse_EURID(WhoisResult whoisResult, StringReader sr)
		{
			string sLine = "";
			StringBuilder errorMessage = new StringBuilder();
			List<string> nameServers = new List<string>();
			bool throwError = true;

			// read 
			while (sLine != null)
			{
				// advance to the next line
				sLine = sr.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Check record status
				if (sLine.StartsWith("Status:"))
				{
                    // cleanup status
                    sLine = sLine.Replace("Status:", "").Trim();
                    // 2. Record found
                    switch (sLine)
                    {
                        case "AVAILABLE":
                            whoisResult.RecordFound = false;
                            return;
                        case "REGISTERED":
                        case "RESERVED":
                            // response detected as success
                            throwError = false;
                            whoisResult.RecordFound = true;
                            break;
                    }
				}

                if (sLine.StartsWith("Registrant:"))
                {
                    // cleanup status
                    sLine = sr.ReadLine();
                    if (!String.IsNullOrEmpty(sLine))
                    {
                        whoisResult.RecordFound = true;
                    }
                }

				// 3. Record found lookup for nameservers
				if (sLine.StartsWith("Nameservers:"))
				{
					// advance to the next line
					sLine = sr.ReadLine();
					// loop for name servers records
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for match
						Match nsMatch = nameServersRegex.Match(sLine);
						// push nameserver to the list
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = sr.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
					return; // EXIT
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw whois error
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

		private static void Parse_ROMANIAN(WhoisResult whoisResult, StringReader sr)
		{
			string sLine = "";
			StringBuilder errorMessage = new StringBuilder();
			List<string> nameServers = new List<string>();
			bool throwError = true;

			// read 
			while (sLine != null)
			{
				// advance to the next line
				sLine = sr.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Check record status
				if (sLine.StartsWith("% No entries found for the selected source(s)."))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Record found
				if (sLine.StartsWith("domain-name:"))
				{
					// response detected as success
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 3. Record found lookup for nameservers
				if (sLine.StartsWith("nameserver:"))
				{
					// loop for name servers records
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for match
						Match nsMatch = nameServersRegex.Match(sLine);
						// push nameserver to the list
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = sr.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
					return; // EXIT
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw whois error
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

		private static void Parse_SWITCH(WhoisResult whoisResult, StringReader sr)
		{
			string sLine = "";
			StringBuilder errorMessage = new StringBuilder();
			List<string> nameServers = new List<string>();
			bool throwError = true;

			// read 
			while (sLine != null)
			{
				// advance to the next line
				sLine = sr.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Check record status
				if (sLine.StartsWith("We do not have an entry in our database matching your query."))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Record found
				if (sLine.StartsWith("Domain name:"))
				{
					// response detected as success
					throwError = false;
					whoisResult.RecordFound = true;
					continue;
				}
				// 3. Record found lookup for nameservers
				if (sLine.StartsWith("Name servers:"))
				{
					// advance to the next line
					sLine = sr.ReadLine();
					// loop for name servers records
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for match
						Match nsMatch = nameServersRegex.Match(sLine);
						// push nameserver to the list
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = sr.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
					return; // EXIT
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw whois error
			if (throwError)
				throw new WhoisException(errorMessage.ToString());
		}

		private static void Parse_NEUSTAR(WhoisResult whoisResult, StringReader sr)
		{
			string sLine = "";
			List<string> nameServers = new List<string>();
			StringBuilder errorMessage = new StringBuilder();
			bool raiseError = true;

			while (sLine != null)
			{
				// advance to the next line
				sLine = sr.ReadLine();
				// skip empty lines
				if (String.IsNullOrEmpty(sLine))
					continue;
				// trim whitespaces
				sLine = sLine.Trim();
				// 1. Record not found
				if (sLine.StartsWith("Not found:"))
				{
					whoisResult.RecordFound = false;
					return; // EXIT
				}
				// 2. Record found
				if (sLine.StartsWith("Domain Name:"))
				{
					whoisResult.RecordFound = true;
					// response detected as success
					raiseError = false;
					continue;
				}
				// 3. Copy nameservers
				if (sLine.StartsWith("Name Server:"))
				{
					while (nameServersRegex.IsMatch(sLine))
					{
						// lookup for nameserver match
						Match nsMatch = nameServersRegex.Match(sLine);
						// add ns record
						nameServers.Add(nsMatch.Value.ToLower());
						// advance to the next line
						sLine = sr.ReadLine();
					}
					// copy result
					whoisResult.NameServers = nameServers.ToArray();
					return; // EXIT
				}
				// 4. Response contains errors
				errorMessage.AppendLine(sLine);
			}
			// throw an error if error message is not empty
			if (raiseError)
				throw new WhoisException(errorMessage.ToString());
		}
	}
}
