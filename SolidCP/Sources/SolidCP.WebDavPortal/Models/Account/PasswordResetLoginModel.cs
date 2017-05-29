using System.ComponentModel.DataAnnotations;
using SolidCP.WebDavPortal.Models.Common;
using SolidCP.WebDavPortal.Resources;

namespace SolidCP.WebDavPortal.Models.Account
{
    public class PasswordResetLoginModel 
    {
        [Required]
        [Display(ResourceType = typeof(Resources.UI), Name = "Email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "EmailInvalid",ErrorMessage = null)]
        public string Email { get; set; }
    }
}