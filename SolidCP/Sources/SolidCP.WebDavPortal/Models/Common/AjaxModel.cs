using System.Collections.Generic;
using SolidCP.WebDavPortal.Models.Common.Enums;

namespace SolidCP.WebDavPortal.Models.Common
{
    public class AjaxModel
    {
        public AjaxModel()
        {
            Messages = new List<Message>();
        }

        public List<Message> Messages { get; private set; }

        public void AddMessage(MessageType type, string value)
        {
            Messages.Add(new Message
            {
                Type = type,
                Value = value
            });
        } 
    }
}