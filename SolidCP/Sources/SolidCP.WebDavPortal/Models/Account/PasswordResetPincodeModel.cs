using System.ComponentModel.DataAnnotations;
using SolidCP.WebDavPortal.Models.Common;

namespace SolidCP.WebDavPortal.Models.Account
{
    public class PasswordResetPincodeModel
    {
        [Required]
        public string Sms { get; set; }
        public bool IsTokenExist { get; set; }
    }
}