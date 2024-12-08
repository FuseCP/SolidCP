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
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SolidCP.Providers.DNS;

namespace SolidCP.Portal
{
    public partial class DomainsImportZone : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Get the domain information
            var domain = ES.Services.Servers.GetDomain(PanelRequest.DomainID);
            //Set the text of the literal to the domain name
            litDomainName.Text = domain.DomainName;
        }
    
        protected void UploadZoneFile_OnClick(object sender, EventArgs e)
        {
            //Get the uploaded zone file
            var zoneFile = file.PostedFile;
            //First check that there was actually a file uploaded
            if (zoneFile != null && zoneFile.ContentLength > 0)
            {
                //Get the contents from the file
                var contents = new StreamReader(zoneFile.InputStream).ReadToEnd();
                try
                {
                    //Get the domain id that gets used throughout the method
                    var domainId = PanelRequest.DomainID;
                    //Try and parse the JSON to an array of DNSRecords
                    var importRecords = JsonConvert.DeserializeObject<DnsRecord[]>(contents);
                    //Get the existing records on the DNS server
                    var existingRecords = ES.Services.Servers.GetDnsZoneRecords(domainId);
                    //Get the records that are new to the zone
                    var newRecords = importRecords.Except(existingRecords);
                    //Loop through the new records
                    foreach (var record in newRecords)
                    {
                        //Add each record
                        var result = ES.Services.Servers.AddDnsZoneRecord(
                            domainId,
                            record.RecordName,
                            record.RecordType,
                            record.RecordData,
                            record.MxPriority,
                            record.SrvPriority,
                            record.SrvWeight,
                            record.SrvPort,
                            record.RecordTTL);
                        //Check if the record couldn't be added for some reason
                        if (result < 0)
                            ShowResultMessage(result);
                    }
                    //Show success message
                    ShowSuccessMessage("DOMAIN_IMPORT");
                }
                catch
                {
                    //Show error message
                    ShowErrorMessage("DOMAIN_IMPORT");
                }
            }
            else
            {
                //Show error message
                ShowErrorMessage("DOMAIN_IMPORT_NO_FILE");
            }
        }
    }
}