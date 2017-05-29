using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SolidCP.Server.Utils;
using SolidCP.WebDavPortal.Models.Common;
using SolidCP.WebDavPortal.Models.Common.Enums;

namespace SolidCP.WebDavPortal.Controllers
{
    public class BaseController : Controller
    {
        public const string MessagesKey = "messagesKey";

        public void AddMessage(MessageType type, string value)
        {
            Log.WriteStart("AddMessage");

            var messages = TempData[MessagesKey] as List<Message>;

            if (messages == null)
            {
                messages = new List<Message>();
            }

            messages.Add(new Message
            {
                Type = type,
                Value = value
            });

            TempData[MessagesKey] = messages;

            Log.WriteEnd("AddMessage");
        }
    }
}