using System.ComponentModel.DataAnnotations;
using SolidCP.Providers.HostedSolution;
using SolidCP.WebDavPortal.Models.Common;
using SolidCP.WebDavPortal.Models.Common.EditorTemplates;

namespace SolidCP.WebDavPortal.Models.Account
{
    public class PasswordChangeModel 
    {
        [Display(ResourceType = typeof (Resources.UI), Name = "OldPassword")]
        [Required(ErrorMessageResourceType = typeof (Resources.Messages), ErrorMessageResourceName = "Required")]
        public string OldPassword { get; set; }

        [UIHint("PasswordEditor")]
        public PasswordEditor PasswordEditor { get; set; }


        public PasswordChangeModel()
        {
            PasswordEditor = new PasswordEditor();
        }
    }
}