using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SolidCP.WebDavPortal.CustomAttributes
{
    public class PhoneNumberAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public const string PhonePattern = @"^\+?\d+$";

        public PhoneNumberAttribute()
            : base(PhonePattern)
        {
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRegexRule(FormatErrorMessage(metadata.GetDisplayName()), Pattern);
        }
    }
}