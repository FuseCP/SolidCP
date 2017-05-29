namespace SolidCP.WebDav.Core
{
    namespace Client
    {
        public interface IConnectionSettings
        {
            bool AllowWriteStreamBuffering { get; set; }
            bool SendChunked { get; set; }
            int TimeOut { get; set; }
        }

        public class WebDavConnectionSettings
        {
            private int _timeOut = 30000;

            public WebDavConnectionSettings()
            {
                SendChunked = false;
                AllowWriteStreamBuffering = false;
            }

            public bool AllowWriteStreamBuffering { get; set; }
            public bool SendChunked { get; set; }

            public int TimeOut
            {
                get { return _timeOut; }
                set { _timeOut = value; }
            }
        }
    }
}