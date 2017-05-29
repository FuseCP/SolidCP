using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.EnterpriseServer
{
    public class OneTimePasswordHelper
    {
        public static string SetOneTimePassword(int userId)
        {
            int passwordLength = 12; // default length

            // load password policy
            UserSettings userSettings = UserController.GetUserSettings(userId, UserSettings.SolidCP_POLICY);
            string passwordPolicy = userSettings["PasswordPolicy"];

            if (!String.IsNullOrEmpty(passwordPolicy))
            {
                // get third parameter - max length
                try
                {
                    passwordLength = Utils.ParseInt(passwordPolicy.Split(';')[2].Trim(), passwordLength);
                }
                catch { /* skip */ }
            }

            // generate password
            var password = Utils.GetRandomString(passwordLength);

            DataProvider.SetUserOneTimePassword(userId, CryptoUtils.Encrypt(password), (int) OneTimePasswordStates.Active);

            return password;
        }

        public static void FireSuccessAuth(UserInfoInternal user)
        {
            DataProvider.SetUserOneTimePassword(user.UserId, CryptoUtils.Encrypt(user.Password), (int) OneTimePasswordStates.Expired);
        }
    }
}
