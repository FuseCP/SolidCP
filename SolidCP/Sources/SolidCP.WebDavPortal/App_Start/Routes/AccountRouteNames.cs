using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidCP.WebDavPortal.UI.Routes
{
    public class AccountRouteNames
    {
        public const string Logout = "AccountLogout";
        public const string Login = "AccountLogin";
        public const string UserProfile = "UserProfileRoute";

        public const string PasswordChange = "PasswordChangeRoute";
        public const string PasswordResetLogin = "PasswordResetLoginRoute";
        public const string PasswordResetPincodeSendOptions = "PasswordResetPincodeSendOptionsRoute";
        public const string PasswordResetPincode = "PasswordResetPincodeRoute";
        public const string PasswordResetFinalStep = "PasswordResetFinalStepRoute";
        public const string PasswordResetSuccess = "PasswordResetSuccess";

        public const string PhoneNumberIsAvailible = "PhoneNumberIsAvailibleRoute";
    }
}