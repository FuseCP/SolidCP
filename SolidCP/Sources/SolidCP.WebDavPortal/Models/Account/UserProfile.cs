using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using SolidCP.WebDavPortal.CustomAttributes;
using SolidCP.WebDavPortal.Models.Common;
using SolidCP.WebDavPortal.UI.Routes;

namespace SolidCP.WebDavPortal.Models.Account
{
    public class UserProfile 
    {
        [Display(ResourceType = typeof(Resources.UI), Name = "PrimaryEmail")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "EmailInvalid", ErrorMessage = null)]
        public string PrimaryEmailAddress { get; set; }

        [Display(ResourceType = typeof(Resources.UI), Name = "DisplayName")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        public string DisplayName { get; set; }
        public string AccountName { get; set; }

        [Display(ResourceType = typeof(Resources.UI), Name = "FirstName")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        public string FirstName { get; set; }
        public string Initials { get; set; }

        [Display(ResourceType = typeof(Resources.UI), Name = "LastName")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Office { get; set; }

        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PhoneNumberInvalid")]
        [UniqueAdPhoneNumber(AccountRouteNames.PhoneNumberIsAvailible, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "AlreadyInUse")]
        [Display(ResourceType = typeof(Resources.UI), Name = "BusinessPhone")]
        public string BusinessPhone { get; set; }

        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PhoneNumberInvalid")]
        [UniqueAdPhoneNumber(AccountRouteNames.PhoneNumberIsAvailible, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "AlreadyInUse")]
        [Display(ResourceType = typeof(Resources.UI), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(Resources.UI), Name = "HomePhone")]
        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PhoneNumberInvalid")]
        [UniqueAdPhoneNumber(AccountRouteNames.PhoneNumberIsAvailible, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "AlreadyInUse")]
        public string HomePhone { get; set; }

        [Display(ResourceType = typeof(Resources.UI), Name = "MobilePhone")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PhoneNumberInvalid")]
        [UniqueAdPhoneNumber(AccountRouteNames.PhoneNumberIsAvailible, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "AlreadyInUse")]
        public string MobilePhone { get; set; }

        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PhoneNumberInvalid")]
        [Display(ResourceType = typeof(Resources.UI), Name = "Pager")]
        [UniqueAdPhoneNumber(AccountRouteNames.PhoneNumberIsAvailible,ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "AlreadyInUse")]
        public string Pager { get; set; }

        [Url(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "UrlInvalid", ErrorMessage = null)]
        public string WebPage { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "EmailInvalid", ErrorMessage = null)]
        public string ExternalEmail { get; set; }

        [UIHint("CountrySelector")]
        public string Country { get; set; }

        public string Notes { get; set; }
        public DateTime PasswordExpirationDateTime { get; set; }
    }
}