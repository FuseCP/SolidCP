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
using System.Globalization;
using System.IO;
using CertEnrollInterop;
using SolidCP.Providers.Common;
using SolidCP.Server.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Web.Administration;

namespace SolidCP.Providers.Web.Iis
{
    public class SSLModuleService80 : SSLModuleService
    {
	    private const string CertificateStoreName = "WebHosting";

        public bool UseSNI { get; private set; }
        public bool UseCCS { get; private set; }
        public string CCSUncPath { get; private set; }
        public string CCSCommonPassword { get; private set; }
        
        public SSLModuleService80(SslFlags sslFlags, string ccsUncPath, string ccsCommonPassword)
        {
            //UseSNI = sslFlags.HasFlag(SslFlags.Sni);
            //UseCCS = sslFlags.HasFlag(SslFlags.CentralCertStore);
            CCSUncPath = ccsUncPath;
            CCSCommonPassword = ccsCommonPassword;
        }

        public new SSLCertificate InstallCertificate(SSLCertificate cert, WebSite website)
        {
            try
            {
                var response = Activator.CreateInstance(Type.GetTypeFromProgID("X509Enrollment.CX509Enrollment", true)) as CX509Enrollment;
                if (response == null)
                {
                    throw new Exception("Cannot create instance of X509Enrollment.CX509Enrollment");
                }

                response.Initialize(X509CertificateEnrollmentContext.ContextMachine);
                response.InstallResponse(
                    InstallResponseRestrictionFlags.AllowUntrustedRoot,
                    cert.Certificate, EncodingType.XCN_CRYPT_STRING_BASE64HEADER,
                    null
                );

                // At this point, certificate has been installed into "Personal" store
                // We need to move it into "WebHosting" store
                // Get certificate
                var servercert = GetServerCertificates(StoreName.My.ToString()).Single(c => c.FriendlyName == cert.FriendlyName);
                
                // Get certificate data - the one we just added to "Personal" store
                var storeMy = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                storeMy.Open(OpenFlags.MaxAllowed);
                X509CertificateCollection existCerts2 = storeMy.Certificates.Find(X509FindType.FindBySerialNumber, servercert.SerialNumber, false);
                var certData = existCerts2[0].Export(X509ContentType.Pfx);
                storeMy.Close();
                var x509Cert = new X509Certificate2(certData, string.Empty, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                if (UseCCS)
                {
                    // Revert to InstallPfx to install new certificate - this also adds binding
                    InstallPfx(certData, string.Empty, website);
                }
                else
                {
                    // Add new certificate to "WebHosting" store
                    var store = new X509Store(CertificateStoreName, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(x509Cert);
                    store.Close();
                }

                // Remove certificate from "Personal" store
                storeMy.Open(OpenFlags.MaxAllowed);
                X509CertificateCollection existCerts = storeMy.Certificates.Find(X509FindType.FindBySerialNumber, servercert.SerialNumber, false);
                storeMy.Remove((X509Certificate2)existCerts[0]);
                storeMy.Close();

                // Fill object with certificate data
                cert.SerialNumber = servercert.SerialNumber;
                cert.ValidFrom = servercert.ValidFrom;
                cert.ExpiryDate = servercert.ExpiryDate;
                cert.Hash = servercert.Hash;
                cert.DistinguishedName = servercert.DistinguishedName;

                if (!UseCCS)
                {
                    if (CheckCertificate(website))
                    {
                        DeleteCertificate(GetCurrentSiteCertificate(website), website);
                    }

                    AddBinding(x509Cert, website);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Error adding SSL certificate", ex);
                cert.Success = false;
            }

            return cert;
        }

        public new List<SSLCertificate> GetServerCertificates()
		{
            // Get certificates from both WebHosting and My (Personal) store
            var certificates = GetServerCertificates(CertificateStoreName);
            certificates.AddRange(GetServerCertificates(StoreName.My.ToString()));
            return certificates;
		}
        
        public new SSLCertificate ImportCertificate(WebSite website)
		{
			SSLCertificate certificate;

			try
			{
			    certificate = GetCurrentSiteCertificate(website);
			}
			catch (Exception ex)
			{
                certificate = new SSLCertificate
                {
                    Success = false, 
                    Certificate = ex.ToString()
                };
			}

            return certificate ?? (new SSLCertificate {Success = false, Certificate = "No certificate in binding on server, please remove or edit binding"});
		}
        
        public new SSLCertificate InstallPfx(byte[] certificate, string password, WebSite website)
		{
            SSLCertificate newcert = null, oldcert = null;

            // Ensure we perform operations safely and preserve the original state during all manipulations, save the oldcert if one is used
		    if (CheckCertificate(website))
		    {
		        oldcert = GetCurrentSiteCertificate(website);
		    }

		    X509Certificate2 x509Cert;
            var store = new X509Store(CertificateStoreName, StoreLocation.LocalMachine);

		    if (UseCCS)
		    {
		        // We need to use this constructor or we won't be able to export this certificate
		        x509Cert = new X509Certificate2(certificate, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

		        var certData = x509Cert.Export(X509ContentType.Pfx);
		        var convertedCert = new X509Certificate2(certData, string.Empty, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

		        // Attempts to move certificate to CCS UNC path
		        try
		        {
		            // Create a stream out of that new certificate
		            certData = convertedCert.Export(X509ContentType.Pfx, CCSCommonPassword);
		            
                    // Open UNC path and set path to certificate subject
		            var filename = (CCSUncPath.EndsWith("/") ? CCSUncPath: CCSUncPath + "/") + x509Cert.GetNameInfo(X509NameType.SimpleName, false) + ".pfx";
		            var writer = new BinaryWriter(File.Open(filename, FileMode.Create));
		            writer.Write(certData);
		            writer.Flush();
		            writer.Close();
		            // Certificate saved
		        }
		        catch (Exception ex)
		        {
		            // Log error
		            Log.WriteError("SSLModuleService could not save certificate to Centralized Certificate Store", ex);
		            // Re-throw
		            throw;
		        }
		    }
		    else
		    {
		        x509Cert = new X509Certificate2(certificate, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

		        // Step 1: Register X.509 certificate in the store
		        // Trying to keep X.509 store open as less as possible
		        try
		        {
		            store.Open(OpenFlags.ReadWrite);
                    store.Add(x509Cert);
		        }
		        catch (Exception ex)
		        {
		            Log.WriteError(String.Format("SSLModuleService could not import PFX into X509Store('{0}', '{1}')", store.Name, store.Location), ex);
		            // Re-throw error
		            throw;
		        }
		        finally
		        {
		            store.Close();
		        }
		    }

		    // Step 2: Instantiate a copy of new X.509 certificate
            try
            {
                newcert = GetSSLCertificateFromX509Certificate2(x509Cert);
            }
            catch (Exception ex)
            {
                HandleExceptionAndRollbackCertificate(store, x509Cert, null, website, "SSLModuleService could not instantiate a copy of new X.509 certificate.", ex);
            }

            // Step 3: Remove old certificate from the web site if any
            try
            {
                // Check if certificate already exists, remove it.
                if (oldcert != null)
                {
                    DeleteCertificate(oldcert, website);
                }
            }
            catch (Exception ex)
            {
                HandleExceptionAndRollbackCertificate(store, x509Cert, null, website, string.Format("SSLModuleService could not remove existing certificate from '{0}' web site.", website.Name), ex);
            }

            // Step 4: Register new certificate with HTTPS binding on the web site
            try
            {
                AddBinding(x509Cert, website);
            }
            catch (Exception ex)
            {
                HandleExceptionAndRollbackCertificate(store, x509Cert, oldcert, website, String.Format("SSLModuleService could not add new X.509 certificate to '{0}' web site.", website.Name), ex);
            }
			
			return newcert;
		}

        public new byte[] ExportPfx(string serialNumber, string password)
        {
            if (UseCCS)
            {
                // This is not a good way to do it
                // Find cert by somehow perhaps first looking in the database? There vi kan lookup the serialnumber and find the hostname needed to create the path to the cert in CCS and then we can load the certdata into a cert and do a export with new password.
                // Another solution would be to look through all SSL-bindings on all sites until we found the site with the binding that has this serialNumber. But serialNumber is not good enough, we need hash that is unique and present in bindingInfo
                // A third solution is to iterate over all files in CCS, load them into memory and find the one with the correct serialNumber, but that cannot be good if there are thousands of files...
                foreach (var file in Directory.GetFiles(CCSUncPath))
                {
		            var fileStream = File.OpenRead(file);

		            // Read certificate data from file
		            var certData = new byte[fileStream.Length];
		            fileStream.Read(certData, 0, (int) fileStream.Length);
		            var convertedCert = new X509Certificate2(certData, CCSCommonPassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                    fileStream.Close();

                    if (convertedCert.SerialNumber == serialNumber)
                    {
                        return convertedCert.Export(X509ContentType.Pfx, password);
                    }
                }
            }

            var store = new X509Store(CertificateStoreName, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, false)[0];
            var exported = cert.Export(X509ContentType.Pfx, password);
            return exported;
        }


        public void AddBinding(X509Certificate2 certificate, WebSite website)
        {
            using (var srvman = GetServerManager())
            {
                // Look for dedicated ip
                var dedicatedIp = SiteHasBindingWithDedicatedIp(srvman, website);

                // Look for all the hostnames this certificate is valid for if we are using SNI
                var hostNames = new List<string>();

                if (!dedicatedIp)
                {
                    hostNames.AddRange(certificate.Extensions.Cast<X509Extension>()
                        .Where(e => e.Oid.Value == "2.5.29.17") // Subject Alternative Names
                        .SelectMany(e => e.Format(true).Split(new[] {"\r\n", "\n", "\n"}, StringSplitOptions.RemoveEmptyEntries).Where(s => s.Contains("=")).Select(s => s.Split('=')[1])).Where(s => !s.Contains(" ")));
                }

                var simpleName = certificate.GetNameInfo(X509NameType.SimpleName, false);
                if (hostNames.All(h => h != simpleName))
                {
                    hostNames.Add(simpleName);
                }

                var wildcardHostName = hostNames.SingleOrDefault(h => h.StartsWith("*."));

                // If a wildcard certificate is used
                if (wildcardHostName != null)
                {
                    if (!dedicatedIp)
                    {
                        // If using a wildcard ssl and not a dedicated IP, we take all the matching bindings on the site and use it to bind to SSL also.
                        hostNames.Remove(wildcardHostName);
                        hostNames.AddRange(website.Bindings.Where(b => !string.IsNullOrEmpty(b.Host) && b.Host.EndsWith(wildcardHostName.Substring(2))).Select(b => b.Host));
                    }
                }

                // For every hostname
                foreach (var hostName in hostNames)
                {
                    var bindingIpAddress = string.IsNullOrEmpty(website.SiteInternalIPAddress) ? website.SiteIPAddress : website.SiteInternalIPAddress;

                    var bindingInformation = string.Format("{0}:443:{1}", bindingIpAddress ?? "*", dedicatedIp ? "" : hostName);

                    Binding siteBinding = UseCCS ? 
                        srvman.Sites[website.SiteId].Bindings.Add(bindingInformation, "https") : 
                        srvman.Sites[website.SiteId].Bindings.Add(bindingInformation, certificate.GetCertHash(), CertificateStoreName);
                
                    if (UseSNI && !dedicatedIp)
                    {
                        siteBinding.SslFlags |= SslFlags.Sni;
                    }
                    if (UseCCS)
                    {
                        siteBinding.SslFlags |= SslFlags.CentralCertStore;
                    }
                }

                srvman.CommitChanges();
            }
        }

		public new ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website)
		{
            // This method removes all https bindings and all certificates associated with them. 
            // Old implementation (IIS70) removed a single binding (there could not be more than one) and the first certificate that matched via serial number
			var result = new ResultObject { IsSuccess = true };

		    if (certificate == null)
		    {
		        return result;
		    }

			try
			{
			    //var certificatesAndStoreNames = new List<Tuple<string, byte[]>>();

                // User servermanager to get aLL SSL-bindings on this website and try to remove the certificates used
			    using (var srvman = GetServerManager())
			    {

			        var site = srvman.Sites[website.Name];
			        var bindings = site.Bindings.Where(b => b.Protocol == "https");

			        foreach (Binding binding in bindings.ToList())
			        {
			            
			            // Remove binding from site
			            site.Bindings.Remove(binding);
			        }

			        srvman.CommitChanges();
			    }
			}
			catch (Exception ex)
			{
                Log.WriteError(String.Format("Unable to delete certificate for website {0}", website.Name), ex);
				result.IsSuccess = false;
				result.AddError("", ex);
			}

			return result;
		}

		public new SSLCertificate GetCurrentSiteCertificate(WebSite website)
		{
		    using (var srvman = GetServerManager())
		    {
		        var site = srvman.Sites[website.SiteId];
		        var sslBinding = site.Bindings.First(b => b.Protocol == "https");

		        // If the certificate is in the central store
		    }

		    return null;
		}

        private static List<SSLCertificate> GetServerCertificates(string certificateStoreName)
		{
            var store = new X509Store(certificateStoreName, StoreLocation.LocalMachine);

            List<SSLCertificate> certificates;
			
			try
			{
				store.Open(OpenFlags.ReadOnly);
			    certificates = store.Certificates.Cast<X509Certificate2>().Select(GetSSLCertificateFromX509Certificate2).ToList();
			}
			catch (Exception ex)
			{
				Log.WriteError(
					String.Format("SSLModuleService is unable to get certificates from X509Store('{0}', '{1}') and complete GetServerCertificates call", store.Name, store.Location), ex);
				// Re-throw exception
				throw;
			}
			finally
			{
				store.Close();
			}

			return certificates;
		}

        private string GetCCSPath(string bindingName)
        {
            return (CCSUncPath.EndsWith("/") ? CCSUncPath : CCSUncPath + "/") + bindingName + ".pfx";
        }

        private static SSLCertificate GetSSLCertificateFromX509Certificate2(X509Certificate2 cert)
        {
            var certificate = new SSLCertificate
            {
                Hostname = cert.GetNameInfo(X509NameType.SimpleName, false),
                FriendlyName = cert.FriendlyName,
                CSRLength = Convert.ToInt32(cert.PublicKey.Key.KeySize.ToString(CultureInfo.InvariantCulture)),
                Installed = true,
                DistinguishedName = cert.Subject,
                Hash = cert.GetCertHash(),
                SerialNumber = cert.SerialNumber,
                ExpiryDate = DateTime.Parse(cert.GetExpirationDateString()),
                ValidFrom = DateTime.Parse(cert.GetEffectiveDateString()),
                Success = true
            };

            return certificate;
        }

        private static bool SiteHasBindingWithDedicatedIp(ServerManager srvman, WebSite website)
        {
            try
            {
                var bindings = srvman.Sites[website.SiteId].Bindings;
                return bindings.Any(b => string.IsNullOrEmpty(b.Host) && b.BindingInformation.Split(':')[1] != "*");
            }
            catch
            {
                return false;
            }
        }

        private void HandleExceptionAndRollbackCertificate(X509Store store, X509Certificate2 x509Cert, SSLCertificate oldCert, WebSite webSite, string errorMessage, Exception ex)
        {
            if (!UseCCS)
            {
                try
                {
                    // Rollback X.509 store changes
                    store.Open(OpenFlags.ReadWrite);
                    store.Remove(x509Cert);
                    store.Close();
                }
                catch (Exception)
                {
                    Log.WriteError("SSLModuleService could not rollback and remove certificate from store", ex);
                }

                // Install old certificate back if any
                if (oldCert != null)
                    InstallCertificate(oldCert, webSite);
            }

            // Log the error
            Log.WriteError(errorMessage + " All changes have been rolled back.", ex);

            // Re-throw
            throw ex;
        }
    }
}
