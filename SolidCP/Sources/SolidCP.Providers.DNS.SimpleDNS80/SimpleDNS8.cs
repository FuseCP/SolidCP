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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SolidCP.Providers.DNS.SimpleDNS80;
using SolidCP.Providers.DNS.SimpleDNS80.Models;
using SolidCP.Providers.DNS.SimpleDNS80.Models.Request;
using SolidCP.Providers.DNS.SimpleDNS80.Models.Response;
using SolidCP.Server.Utils;

// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

namespace SolidCP.Providers.DNS
{
    public class SimpleDNS8 : HostingServiceProviderBase, IDnsServer
    {
        #region Properties
        private int ExpireLimit => ProviderSettings.GetInt("ExpireLimit");

        public int MinimumTTL => ProviderSettings.GetInt("MinimumTTL");

        private int RefreshInterval => ProviderSettings.GetInt("RefreshInterval");

        private int RetryDelay => ProviderSettings.GetInt("RetryDelay");

        private string SimpleDnsUrl => ProviderSettings["SimpleDnsUrl"];

        private string SimpleDnsUsername => ProviderSettings["AdminLogin"];

        private string SimpleDnsPassword => ProviderSettings["Password"];

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Method that will return a correctly configured HttpClient for methods to use
        /// </summary>
        /// <returns><see cref="HttpClient"/></returns>
        private HttpClient GetApiClient()
        {
            //Create the client that will be returned
            var client = new HttpClient();
            //Set the base address of the client
            client.BaseAddress = new Uri($"{SimpleDnsUrl}/v2/");
            //Check whether or not to add authorization headers
            if (!string.IsNullOrWhiteSpace(SimpleDnsUsername))
            {
                //Get byte array of the username password combo
                var byteArray = Encoding.ASCII.GetBytes($"{SimpleDnsUsername}:{SimpleDnsPassword}");
                //Add the authorization header to the client
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }
            //Return the defined client
            return client;
        }

        /// <summary>
        /// Method to make a GET request to the SimpleDNS API
        /// </summary>
        /// <param name="endpoint">API endpoint to GET from</param>
        /// <exception cref="ArgumentNullException"></exception>
        private string ApiGet(string endpoint)
        {
            //Null check the endpoint
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentNullException(nameof(endpoint));
            //Get the client to work with
            var client = GetApiClient();
            //Try to get the response from the API and handle possible exceptions
            try
            {
                //Get the response from the API
                var response = client.GetAsync(endpoint).Result;
                //Get the content from the response
                var content = response.Content.ReadAsStringAsync().Result;
                //Check if the call was successful
                if (!response.IsSuccessStatusCode)
                    Log.WriteWarning($"SimpleDNS 8 API Call (GET) Failed - {content}");
                //Return the string result
                return content;
            }
            catch (InvalidOperationException ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (GET) - HttpClient already sent GET request.", ex);
                return null;
            }
            catch (HttpRequestException ex)
            {
                //Handle error and return null
                Log.WriteError(
                    "SimpleDNS 8 API Call (GET) - HttpClient GET request failed - network connectivity, DNS failure, server certificate validation or timeout.",
                    ex);
                return null;
            }
            catch (TaskCanceledException ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (GET) - HttpClient timed out.", ex);
                return null;
            }
            catch (Exception ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (GET) - An unhandled exception occured.", ex);
                return null;
            }
            finally
            {
                //Dispose of the HttpClient
                client.Dispose();
            }
        }

        /// <summary>
        /// Method to make a PUT request to the SimpleDNS API
        /// </summary>
        /// <param name="endpoint">API endpoint to PUT to</param>
        /// <param name="jsonBody">JSON string of the body that should be PUT'ed</param>
        /// <exception cref="ArgumentNullException"></exception>
        private string ApiPut(string endpoint, string jsonBody)
        {
            //Null checks
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentNullException(nameof(endpoint));
            if (string.IsNullOrWhiteSpace(jsonBody))
                throw new ArgumentNullException(nameof(jsonBody));

            //Get the client to work with
            var client = GetApiClient();
            //Try to get the response from the API and handle possible exceptions
            try
            {
                //Build up the content
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                //PUT the content to the API
                var response = client.PutAsync(endpoint, content).Result;
                //Check if the PUT was successful
                var responseContent = response.Content.ReadAsStringAsync().Result;
                //Check if there was an error from the API
                if (!response.IsSuccessStatusCode)
                    Log.WriteWarning($"SimpleDNS 8 API Call (PUT) Failed - {responseContent}");
                //Return the content
                return responseContent;
            }
            catch (InvalidOperationException ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (PUT) - HttpClient already sent GET request.", ex);
                return null;
            }
            catch (HttpRequestException ex)
            {
                //Handle error and return null
                Log.WriteError(
                    "SimpleDNS 8 API Call (PUT) - HttpClient PUT request failed - network connectivity, DNS failure, server certificate validation or timeout.",
                    ex);
                return null;
            }
            catch (TaskCanceledException ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (PUT) - HttpClient timed out.", ex);
                return null;
            }
            catch (Exception ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (PUT) - An unhandled exception occured.", ex);
                return null;
            }
            finally
            {
                //Dispose of the HttpClient
                client.Dispose();
            }
        }

        /// <summary>
        /// Method to make a DELETE request to the SimpleDNS API
        /// </summary>
        /// <param name="endpoint">API endpoint for DELETE</param>
        /// <exception cref="ArgumentNullException"></exception>
        private string ApiDelete(string endpoint)
        {
            //Null checks
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentNullException(nameof(endpoint));

            //Get the client to work with
            var client = GetApiClient();
            //Try to get the response from the API and handle possible exceptions
            try
            {
                //Make DELETE call to API endpoint
                var response = client.DeleteAsync(endpoint).Result;
                //Check if the PUT was successful
                var responseContent = response.Content.ReadAsStringAsync().Result;
                //Check if there was an error from the API
                if (!response.IsSuccessStatusCode)
                    Log.WriteWarning($"SimpleDNS 8 API Call (DELETE) Failed - {responseContent}");
                //Return the content
                return responseContent;
            }
            catch (InvalidOperationException ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (DELETE) - HttpClient already sent GET request.", ex);
                return null;
            }
            catch (HttpRequestException ex)
            {
                //Handle error and return null
                Log.WriteError(
                    "SimpleDNS 8 API Call (DELETE) - HttpClient PUT request failed - network connectivity, DNS failure, server certificate validation or timeout.",
                    ex);
                return null;
            }
            catch (TaskCanceledException ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (DELETE) - HttpClient timed out.", ex);
                return null;
            }
            catch (Exception ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (DELETE) - An unhandled exception occured.", ex);
                return null;
            }
            finally
            {
                //Dispose of the HttpClient
                client.Dispose();
            }
        }

        /// <summary>
        /// Method to make a PATCH request to the SimpleDNS API
        /// </summary>
        /// <param name="endpoint">API endpoint to PATCH to</param>
        /// <param name="jsonBody">JSON string of the body that should be patched</param>
        /// <exception cref="ArgumentNullException"></exception>
        private string ApiPatch(string endpoint, string jsonBody)
        {
            //Null checks
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentNullException(nameof(endpoint));
            if (string.IsNullOrWhiteSpace(jsonBody))
                throw new ArgumentNullException(nameof(jsonBody));

            //Get the client to work with
            var client = GetApiClient();
            //Try to get the response from the API and handle possible exceptions
            try
            {
                //Build up the content
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                //PUT the content to the API
                var response = client.PatchAsync(new Uri($"{SimpleDnsUrl}/v2/{endpoint}"), content).Result;
                //Check if the PUT was successful
                var responseContent = response.Content.ReadAsStringAsync().Result;
                //Check if there was an error from the API
                if (!response.IsSuccessStatusCode)
                    Log.WriteWarning($"SimpleDNS 8 API Call (PATCH) Failed - {responseContent}");
                //Return the content
                return responseContent;
            }
            catch (InvalidOperationException ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (PATCH) - HttpClient already sent GET request.", ex);
                return null;
            }
            catch (HttpRequestException ex)
            {
                //Handle error and return null
                Log.WriteError(
                    "SimpleDNS 8 API Call (PATCH) - HttpClient PUT request failed - network connectivity, DNS failure, server certificate validation or timeout.",
                    ex);
                return null;
            }
            catch (TaskCanceledException ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (PATCH) - HttpClient timed out.", ex);
                return null;
            }
            catch (Exception ex)
            {
                //Handle error and return null
                Log.WriteError("SimpleDNS 8 API Call (PATCH) - An unhandled exception occured.", ex);
                return null;
            }
            finally
            {
                //Dispose of the HttpClient
                client.Dispose();
            }
        }

        #endregion

        #region Public Methods

        public SimpleDNS8()
        {
            
        }

        #endregion

        public override bool IsInstalled()
        {
            //Call statistics endpoint
            var response = ApiGet("statistics");
            StatisticsResponse statisticsResponse;
            //Try deserialize JSON response as response object
            try
            {
                statisticsResponse = StatisticsResponse.FromJson(response);
            }
            catch
            {
                //If the JSON cannot be deserialized then there is a problem with the API, so we return false
                return false;
            }
            //Check the API status
            var value = statisticsResponse.Sections.FirstOrDefault(sec => sec.Id == "general")?.Items.FirstOrDefault(item => item.Id == "state")?.Value;
            //Check if the value is one
            return value == 1;
        }

        public bool ZoneExists(string zoneName)
        {
            //Null check the zoneName
            if (string.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException(nameof(zoneName));
            //Call zone endpoint
            var response = ApiGet($"zones/{zoneName}");
            //Check the response
            return !response.Contains("Zone not found");
        }

        public string[] GetZones()
        {
            //Call the API endpoint for all zones
            var response = ApiGet("zones");
            //Deserialize JSON to object
            var result = ZoneResponse.FromJson(response);
            //Return array of zones
            return result.Select(x => x.Name).ToArray();
        }

        public void AddPrimaryZone(string zoneName, string[] secondaryServers)
        {
            //Null checking
            if (string.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException(nameof(zoneName));

            //Call the API endpoint to create the zone
            ApiPut($"zones/{zoneName}", "{}");

            //TODO: Check if I need to do anything with secondary servers
        }

        public void AddSecondaryZone(string zoneName, string[] masterServers)
        {
            //Null checking
            if (string.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException(nameof(zoneName));
            if (masterServers.Length == 0 || masterServers[0] == null)
                throw new ArgumentNullException(nameof(masterServers));

            //Check if a valid IP address was provided for the secondary zone
            if (IPAddress.TryParse(masterServers[0], out _))
                throw new ArgumentOutOfRangeException(nameof(masterServers), "A valid IP address was not provided.");

            //Create the object to later serialize into JSON
            var request = new SecondaryZoneRequest
            {
                Type = "Secondary",
                PrimaryIp = masterServers[0]
            };

            //Call the API endpoint to create the secondary zone
            ApiPut($"zones/{zoneName}", request.ToJson());
        }

        public void DeleteZone(string zoneName)
        {
            //Null checking
            if (string.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException(nameof(zoneName));

            //Check if Zone exists
            if (!ZoneExists(zoneName)) return;

            //Call the API endpoint to delete Zone
            ApiDelete($"zones/{zoneName}");
        }

        public void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
            //Null checking
            if (string.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException(nameof(zoneName));
            if (string.IsNullOrWhiteSpace(primaryNsServer))
                throw new ArgumentNullException(nameof(primaryNsServer));
            if (string.IsNullOrWhiteSpace(primaryPerson))
                throw new ArgumentNullException(nameof(primaryPerson));

            //Define the endpoint since it is used multiple times
            var endpoint = $"zones/{zoneName}/records";

            //Get all the existing records 
            //We need to get all the records according to the new API specification, as the PUT request updates the entire zones records - not just a single one
            //New API will automatically implement SOA serial number so we can pass a 0 on in its place
            var records = ZoneRecordsResponse.FromJson(ApiGet(endpoint));
            //Get the SOA record and edit it. Can disable ReSharper warning as there is always a SOA record
            //ReSharper disable once PossibleNullReferenceException
            records.FirstOrDefault(record => record.Type == "SOA").Data =
                $"{Utilities.CorrectSOARecord(zoneName, primaryNsServer)} {Utilities.CorrectSOARecord(zoneName, primaryPerson)} 0 {RefreshInterval} {RetryDelay} {ExpireLimit} {MinimumTTL}";
            //PUT the records back to the API
            ApiPut(endpoint, records.ToJson());
        }

        public DnsRecord[] GetZoneRecords(string zoneName)
        {
            //Null checking
            if (string.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException(nameof(zoneName));

            //Get all the records for the specific zone
            var records = ZoneRecordsResponse.FromJson(ApiGet($"zones/{zoneName}/records"));

            //Return the resulting array
            return records.ToDnsRecordArray();
        }

        public void AddZoneRecord(string zoneName, DnsRecord record)
        {
            //Null checking
            if (string.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException(nameof(zoneName));
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            //Declare content to be Patched
            var content = new List<ZoneRecordsResponse>
            {
                //Add DnsRecord to it
                record.ToZoneRecordsResponse(MinimumTTL, zoneName)
            };

            Log.WriteInfo("AddZoneRecord content: {0}", content.ToJson());

            //Call API to PATCH record
            ApiPatch($"zones/{zoneName}/records", content.ToJson());
        }

        public void AddZoneRecords(string zoneName, DnsRecord[] records)
        {
            //Null checking
            if (string.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException(nameof(zoneName));
            if (records.Length == 0 || records[0] == null)
                return;

            //Declare content to be patched
            var content = records.Select(record => record.ToZoneRecordsResponse(MinimumTTL, zoneName)).ToList();

            //Call API to PATCH records
            ApiPatch($"zones/{zoneName}/records", content.ToJson());
        }

        public void DeleteZoneRecord(string zoneName, DnsRecord record)
        {
            //Null checking
            if (string.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException(nameof(zoneName));
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            //Declare content to be patched
            var content = new List<ZoneRecordsDeleteRequest>
            {
                //Convert record into delete request
                new ZoneRecordsDeleteRequest(record.ToZoneRecordsResponse(MinimumTTL, zoneName))
            };

            //Call API to PATCH record
            ApiPatch($"zones/{zoneName}/records", content.ToJson());
        }

        public void DeleteZoneRecords(string zoneName, DnsRecord[] records)
        {
            //Null checking
            if (string.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException(nameof(zoneName));
            if (records.Length == 0 || records[0] == null)
                throw new ArgumentNullException(nameof(records));

            //Build up list of delete requests
            var deleteRequests = records.Select(record => new ZoneRecordsDeleteRequest(record.ToZoneRecordsResponse(MinimumTTL, zoneName))).ToList();

            //Call API to PATCH record
            ApiPatch($"zones/{zoneName}/records", deleteRequests.ToJson());
        }
    }
}
