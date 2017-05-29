using System;

namespace SolidCP.WebDav.Core.Exceptions
{
    public class WebDavException : Exception
    {
        public WebDavException()
            : base() { }

        public WebDavException(string message)
            : base(message) { }

        public WebDavException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public WebDavException(string message, Exception innerException)
            : base(message, innerException) { }

        public WebDavException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}