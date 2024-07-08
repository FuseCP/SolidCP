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
    public class UserHelper: ControllerBase
    {
        public UserHelper(ControllerBase provider): base(provider) { }

        public UserInfoInternal GetUser(string username)
        {
            return GetUser(Database.GetUserByUsernameInternally(username));
        }

        public UserInfoInternal GetUser(int userId)
        {
            return GetUser(Database.GetUserById(SecurityContext.User.UserId, userId));
        }

        private UserInfoInternal GetUser(IDataReader reader)
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
