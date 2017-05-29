using System;
using SolidCP.WebDavPortal.Models.Common;

namespace SolidCP.WebDavPortal.Models
{
    public class ErrorModel
    {
        public int HttpStatusCode { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}