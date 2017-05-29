using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using SolidCP.WebDav.Core;

namespace SolidCP.WebDavPortal.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class UniqueAdPhoneNumberAttribute : RemoteAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueString = value as string;

            if (!string.IsNullOrEmpty(valueString) && ScpContext.User != null)
            {
                var attributes =
                    validationContext.ObjectType.GetProperty(validationContext.MemberName)
                        .GetCustomAttributes(typeof(DisplayNameAttribute), true);

                string displayName = attributes != null && attributes.Any()
                    ? (attributes[0] as DisplayNameAttribute).DisplayName
                    : validationContext.DisplayName;


                var result = !ScpContext.Services.Organizations.CheckPhoneNumberIsInUse(ScpContext.User.ItemId, valueString, ScpContext.User.Login);

                return result ? ValidationResult.Success :
                       new ValidationResult(string.Format(Resources.Messages.AlreadyInUse, displayName));
            }

            return ValidationResult.Success;
        }

        public UniqueAdPhoneNumberAttribute(string routeName) : base(routeName) { }
        public UniqueAdPhoneNumberAttribute(string action, string controller) : base(action, controller) { }
        public UniqueAdPhoneNumberAttribute(string action, string controller, 
               string area) : base(action, controller, area) { }
    }
}