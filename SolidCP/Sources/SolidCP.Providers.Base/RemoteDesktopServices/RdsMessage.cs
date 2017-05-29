using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.Providers.RemoteDesktopServices
{
    public class RdsMessage
    {
        public int Id { get; set; }
        public int RdsCollectionId { get; set; }
        public string MessageText { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
}
