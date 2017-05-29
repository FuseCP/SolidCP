using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using SolidCP.EnterpriseServer;


namespace SolidCP.EnterpriseServer
{
    public class UserHelper
    {
        public static UserInfoInternal GetUser(string username)
        {
            return GetUser(DataProvider.GetUserByUsernameInternally(username));
        }

        public static UserInfoInternal GetUser(int userId)
        {
            return GetUser(DataProvider.GetUserById(SecurityContext.User.UserId, userId));
        }

        private static UserInfoInternal GetUser(IDataReader reader)
        {
            // try to get user from database
            UserInfoInternal user = ObjectUtils.FillObjectFromDataReader<UserInfoInternal>(reader);

            if (user != null)
            {
                user.Password = CryptoUtils.Decrypt(user.Password);
            }
            return user;
        }

    }
}
