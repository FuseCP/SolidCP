using SolidCP.WebDavPortal.Models.Common.Enums;

namespace SolidCP.WebDavPortal.Models.Common
{
    public class Message
    {
        public MessageType Type {get;set;}
        public string Value { get; set; }
    }
}