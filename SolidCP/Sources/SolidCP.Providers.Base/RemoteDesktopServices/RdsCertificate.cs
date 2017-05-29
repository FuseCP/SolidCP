using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.Providers.RemoteDesktopServices
{
    public class RdsCertificate
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public string Hash { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
