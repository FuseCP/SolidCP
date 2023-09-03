// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;
using Google.Authenticator;

namespace SolidCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for UserController.
	/// </summary>
	public class UserController
	{
		public static bool UserExists(string username)
		{
			// try to get user from database
			UserInfo user = GetUserInternally(username);
			return (user != null);
		}

        public static int AuthenticateUser(string username, string password, string ip)
        {
            // start task
            TaskManager.StartTask("USER", "AUTHENTICATE", username);
            TaskManager.WriteParameter("IP", ip);

            try
            {
                int result = 0;

                // try to get user from database
                UserInfoInternal user = GetUserInternally(username);

                // check if the user exists
                if (user == null)
                {
                    TaskManager.WriteWarning("Wrong username");
                    return BusinessErrorCodes.ERROR_USER_WRONG_USERNAME;
                }

                // check if the user is disabled
                if (user.LoginStatus == UserLoginStatus.Disabled)
                {
                    TaskManager.WriteWarning("User disabled");
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_DISABLED;
                }

                // check if the user is locked out
                if (user.LoginStatus == UserLoginStatus.LockedOut)
                {
                    TaskManager.WriteWarning("User locked out");
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_LOCKEDOUT;
                }

                //Get the password policy
                UserSettings userSettings = UserController.GetUserSettings(user.UserId, UserSettings.SolidCP_POLICY);
                int lockOut = -1;

                if (!string.IsNullOrEmpty(userSettings["PasswordPolicy"]))
                {
                    string passwordPolicy = userSettings["PasswordPolicy"];
                    try
                    {
                        // parse settings
                        string[] parts = passwordPolicy.Split(';');
                        lockOut = Convert.ToInt32(parts[7]);
                    }
                    catch { /* skip */ }
                }


                // compare user passwords
                if ((CryptoUtils.SHA1(user.Password) == password) || (user.Password == password))
                {
                    switch (user.OneTimePasswordState)
                    {
                        case OneTimePasswordStates.Active:
                            result = BusinessSuccessCodes.SUCCESS_USER_ONETIMEPASSWORD;
                            OneTimePasswordHelper.FireSuccessAuth(user);
                            break;
                        case OneTimePasswordStates.Expired:
                            if (lockOut >= 0) DataProvider.UpdateUserFailedLoginAttempt(user.UserId, lockOut, false);
                            TaskManager.WriteWarning("Expired one time password");
                            return BusinessErrorCodes.ERROR_USER_EXPIRED_ONETIMEPASSWORD;
                            break;
                    }
                }
                else
                {
                    if (lockOut >= 0)
                        DataProvider.UpdateUserFailedLoginAttempt(user.UserId, lockOut, false);

                    TaskManager.WriteWarning("Wrong password");
                    return BusinessErrorCodes.ERROR_USER_WRONG_PASSWORD;  
                }
                    
                DataProvider.UpdateUserFailedLoginAttempt(user.UserId, lockOut, true);

                // check status
                if (user.Status == UserStatus.Cancelled)
                {
                    TaskManager.WriteWarning("Account cancelled");
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_CANCELLED;
                }

                if (user.Status == UserStatus.Pending)
                {
                    TaskManager.WriteWarning("Account pending");
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_PENDING;
                }

				// if onetimepassword no MFA
				if (user.MfaMode > 0 && result != BusinessSuccessCodes.SUCCESS_USER_ONETIMEPASSWORD)
				{
					if(user.MfaMode == 1)
                    {
						SendVerificationCode(username);
					}
					TaskManager.Write("MFA active");
					return BusinessSuccessCodes.SUCCESS_USER_MFA_ACTIVE;
				}
				
				return result;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

		public static bool ValidatePin(string username, string pin, TimeSpan timeTolerance)
		{
			UserInfoInternal user = GetUserInternally(username);
			TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
			var isValid = twoFactorAuthenticator.ValidateTwoFactorPIN(CryptoUtils.Decrypt(user.PinSecret), pin, timeTolerance);
			return isValid;
		}

		private static string[] GetCurrentPins(UserInfoInternal user, TimeSpan timeTolerance)
        {
			TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
			var pins = twoFactorAuthenticator.GetCurrentPINs(CryptoUtils.Decrypt(user.PinSecret), timeTolerance);
			return pins;
		}

		private static string GetCurrentPin(UserInfoInternal user)
		{
			TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
			var pin = twoFactorAuthenticator.GetCurrentPIN(CryptoUtils.Decrypt(user.PinSecret));
			return pin;
		}

		private static string GetRandomString(int length)
		{
			Random random = new Random(Guid.NewGuid().GetHashCode());
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!§$%&/()=?";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static bool UpdateUserMfaSecret(string username, bool activate)
		{
			UserInfoInternal user = GetUserInternally(username);

			var authSettings = SystemController.GetSystemSettingsInternal(SystemSettings.AUTHENTICATION_SETTINGS, false);
			var canPeerChangeMfa = Convert.ToBoolean(authSettings[SystemSettings.MFA_CAN_PEER_CHANGE_MFA]);

			var canChange = DataProvider.CanUserChangeMfa(SecurityContext.User.UserId, user.UserId, canPeerChangeMfa);

			System.Diagnostics.Debug.WriteLine($"canPeerChangeMfa {canPeerChangeMfa} / canhange {canChange}");

			if (!canChange)
				return false;

			var pinSecret = activate ? CryptoUtils.Encrypt(GetRandomString(20)) : null;
			DataProvider.UpdateUserPinSecret(SecurityContext.User.UserId, user.UserId, pinSecret);
			DataProvider.UpdateUserMfaMode(SecurityContext.User.UserId, user.UserId, pinSecret != null ? 1 : 0);

			UserInfoInternal userAfterUpdate = GetUserInternally(username);
			return userAfterUpdate.MfaMode > 0;
		}

		public static string[] GetUserMfaQrCodeData(string username)
		{
			UserInfoInternal user = GetUserInternally(username);

			if (user.MfaMode == 0)
				return new string[0];

			var authSettings = SystemController.GetSystemSettingsInternal(SystemSettings.AUTHENTICATION_SETTINGS, false);
			var mfaTokenAppDisplayName = authSettings == null ? "SolidCP" : authSettings[SystemSettings.MFA_TOKEN_APP_DISPLAY_NAME];

			TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
			var faSetupCode = twoFactorAuthenticator.GenerateSetupCode(mfaTokenAppDisplayName, $"{user.Username}", CryptoUtils.Decrypt(user.PinSecret), false);
			return new string[] { faSetupCode.ManualEntryKey, faSetupCode.QrCodeSetupImageUrl };
		}

		public static bool ActivateUserMfaQrCode(string username, string pin)
		{
			UserInfoInternal user = GetUserInternally(username);
			if(user.MfaMode == 0)
				return false;

			var valid = ValidatePin(username, pin, TimeSpan.FromSeconds(60));
			if (!valid)
				return false;

			DataProvider.UpdateUserMfaMode(SecurityContext.User.UserId, user.UserId, 2);
			return true;
		}
		
		public static bool CanUserChangeMfa(int changeUserId)
		{
			var currentUserId = SecurityContext.User.UserId;
			var authSettings = SystemController.GetSystemSettingsInternal(SystemSettings.AUTHENTICATION_SETTINGS, false);
			var canPeerChangeMfa = Convert.ToBoolean(authSettings[SystemSettings.MFA_CAN_PEER_CHANGE_MFA]);

			return DataProvider.CanUserChangeMfa(currentUserId, changeUserId, canPeerChangeMfa);
		}

		public static UserInfo GetUserByUsernamePassword(string username, string password, string ip)
		{
			// place log record
			// TaskManager create backgroundtasklogs in db and them immediately remove and move them into auditlog (if it is a short task).
			// The TaskManager is great for long tasks, but for short tasks that are called every second (from SOAP calls, for example) it puts a huge load on the DB.
			//TaskManager.StartTask("USER", "GET_BY_USERNAME_PASSWORD", username);
			//TaskManager.WriteParameter("IP", ip);

			try
			{
				// try to get user from database
				UserInfoInternal user = GetUserInternally(username);

				// check if the user exists
				if (user == null)
				{
					//TaskManager.WriteWarning("Account not found");
					AuditLog.AddAuditLogWarningRecord("USER", "GET_BY_USERNAME_PASSWORD", username, new string[] { "IP: " + ip, "Account not found" });
					return null;
				}

				// compare user passwords
                if ((CryptoUtils.SHA1(user.Password) == password) || (user.Password == password))
                {
					AuditLog.AddAuditLogInfoRecord("USER", "GET_BY_USERNAME_PASSWORD", username, new string[] { "IP: " + ip });
					return new UserInfo(user);
				}
					

				return null;
			}
			catch (Exception ex)
			{
				AuditLog.AddAuditLogErrorRecord("USER", "GET_BY_USERNAME_PASSWORD", username, 
					new string[] { 
						"IP: " + ip,
						"Message: " + ex.Message,
						"StackTrace: " + ex.StackTrace
					});
				//throw TaskManager.WriteError(ex);
				throw ex;
			}
            //finally
            //{
            //    TaskManager.CompleteTask();
            //}
        }

        public static int ChangeUserPassword(string username, string oldPassword, string newPassword, string ip)
		{
			// place log record
			TaskManager.StartTask("USER", "CHANGE_PASSWORD_BY_USERNAME_PASSWORD", username);
			TaskManager.WriteParameter("IP", ip);

			try
			{
				UserInfo user = GetUserByUsernamePassword(username, oldPassword, ip);
				if (user == null)
				{
					TaskManager.WriteWarning("Account not found");
					return BusinessErrorCodes.ERROR_USER_NOT_FOUND;
				}

				// change password
				DataProvider.ChangeUserPassword(-1, user.UserId,
					CryptoUtils.Encrypt(newPassword));

				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int SendPasswordReminder(string username, string ip)
		{
			// place log record
			TaskManager.StartTask("USER", "SEND_REMINDER", username);
			TaskManager.WriteParameter("IP", ip);

			try
			{
				// try to get user from database
				UserInfoInternal user = GetUserInternally(username);
				if (user == null)
				{
					TaskManager.WriteWarning("Account not found");
					// Fix for item #273 (NGS-9)
					//return BusinessErrorCodes.ERROR_USER_NOT_FOUND;
					return 0;
				}

				UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.PASSWORD_REMINDER_LETTER);
				string from = settings["From"];
				string cc = settings["CC"];
				string subject = settings["Subject"];
				string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
				bool isHtml = user.HtmlMail;

				MailPriority priority = MailPriority.Normal;
				if (!String.IsNullOrEmpty(settings["Priority"]))
					priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);

				if (body == null || body == "")
					return BusinessErrorCodes.ERROR_SETTINGS_PASSWORD_LETTER_EMPTY_BODY;
                
                // One Time Password feature
                user.Password = OneTimePasswordHelper.SetOneTimePassword(user.UserId);

                // set template context items
                Hashtable items = new Hashtable();
                items["user"] = user;
                items["Email"] = true;

			    // get reseller details
				UserInfoInternal reseller = UserController.GetUser(user.OwnerId);
				if (reseller != null)
				{
					items["reseller"] = new UserInfo(reseller);
				}

				subject = PackageController.EvaluateTemplate(subject, items);
				body = PackageController.EvaluateTemplate(body, items);

				// send message
				MailHelper.SendMessage(from, user.Email, cc, subject, body, priority, isHtml);

				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int SendVerificationCode(string username)
		{
			// place log record
			TaskManager.StartTask("USER", "SEND_VERIFICATION_CODE", username);

			try
			{
				// try to get user from database
				UserInfoInternal user = GetUserInternally(username);
				if (user == null)
				{
					TaskManager.WriteWarning("Account not found");
					return 1;
				}

				if(user.MfaMode == 2)
                {
					TaskManager.WriteWarning("Mode is 2 Email will not be sent");
					return 2;
				}
				

				UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.VERIFICATION_CODE_LETTER);
				string from = settings["From"];
				string cc = settings["CC"];
				string subject = settings["Subject"];
				string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
				bool isHtml = user.HtmlMail;

				MailPriority priority = MailPriority.Normal;
				if (!String.IsNullOrEmpty(settings["Priority"]))
					priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);

				if (string.IsNullOrWhiteSpace(body))
					return BusinessErrorCodes.ERROR_SETTINGS_PASSWORD_LETTER_EMPTY_BODY;

				// set template context items
				Hashtable items = new Hashtable();
				items["user"] = user;
				items["verificationCode"] = GetCurrentPin(user);

				subject = PackageController.EvaluateTemplate(subject, items);
				body = PackageController.EvaluateTemplate(body, items);

				// send message
				MailHelper.SendMessage(from, user.Email, cc, subject, body, priority, isHtml);

				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}


		internal static UserInfoInternal GetUserInternally(int userId)
		{
            return GetUser(DataProvider.GetUserByIdInternally(userId));
		}

		internal static UserInfoInternal GetUserInternally(string username)
		{
            return GetUser(DataProvider.GetUserByUsernameInternally(username));
		}

        public static UserInfoInternal GetUser(int userId)
		{
            return GetUser(DataProvider.GetUserById(SecurityContext.User.UserId, userId));
		}

        public static UserInfoInternal GetUser(string username)
		{
            return GetUser(DataProvider.GetUserByUsername(SecurityContext.User.UserId, username));
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

		public static List<UserInfo> GetUserParents(int userId)
		{
			// get users from database
			DataSet dsUsers = DataProvider.GetUserParents(SecurityContext.User.UserId, userId);

			// convert to arraylist
			List<UserInfo> users = new List<UserInfo>();
			ObjectUtils.FillCollectionFromDataSet<UserInfo>(users, dsUsers);
			return users;
		}

		public static List<UserInfo> GetUsers(int userId, bool recursive)
		{
			// get users from database
			DataSet dsUsers = DataProvider.GetUsers(SecurityContext.User.UserId, userId, recursive);

			// convert to arraylist
			List<UserInfo> users = new List<UserInfo>();
			ObjectUtils.FillCollectionFromDataSet<UserInfo>(users, dsUsers);
			return users;
		}

		public static DataSet GetUsersPaged(int userId, string filterColumn, string filterValue,
			int statusId, int roleId,
			string sortColumn, int startRow, int maximumRows)
		{
			// get users from database
			return DataProvider.GetUsersPaged(SecurityContext.User.UserId, userId,
				filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows, false);
		}

		public static DataSet GetUsersPagedRecursive(int userId, string filterColumn, string filterValue,
			int statusId, int roleId,
			string sortColumn, int startRow, int maximumRows)
		{
			// get users from database
			return DataProvider.GetUsersPaged(SecurityContext.User.UserId, userId,
				filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows, true);
		}

		public static DataSet GetUsersSummary(int userId)
		{
			return DataProvider.GetUsersSummary(SecurityContext.User.UserId, userId);
		}

		public static DataSet GetUserDomainsPaged(int userId, string filterColumn, string filterValue,
			string sortColumn, int startRow, int maximumRows)
		{
			// get users from database
			return DataProvider.GetUserDomainsPaged(SecurityContext.User.UserId, userId,
				filterColumn, filterValue, sortColumn, startRow, maximumRows);
		}

		public static List<UserInfo> GetUserPeers(int userId)
		{
			// get user peers from database
			return ObjectUtils.CreateListFromDataSet<UserInfo>(GetRawUserPeers(userId));
		}

		public static DataSet GetRawUserPeers(int userId)
		{
			// get user peers from database
			return DataProvider.GetUserPeers(SecurityContext.User.UserId, userId);
		}

		public static DataSet GetRawUsers(int ownerId, bool recursive)
		{
			// get users from database
			return DataProvider.GetUsers(SecurityContext.User.UserId, ownerId, recursive);
		}

		public static int AddUser(UserInfo user, bool sendLetter, string password, string[] notes)
		{
			int userId = AddUser(user, sendLetter, password);

			if (userId > 0 && notes != null)
			{
				foreach(string note in notes)
				{
					// user added successfully, save the notes
					DataProvider.AddComment(
						SecurityContext.User.UserId,
						"USER", 
						userId,
						note, 
						1
						);
				}

			}
			
			return userId;
		}

		public static int AddUser(UserInfo user, bool sendLetter, string password)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			string pattern = @"[\\/:*?<>|""]+";

			if (String.IsNullOrEmpty(user.Username))
				return BusinessErrorCodes.ERROR_INVALID_USER_NAME;

			if (Regex.IsMatch(user.Username, pattern))
				return BusinessErrorCodes.ERROR_INVALID_USER_NAME;

			if (UserExists(user.Username))
				return BusinessErrorCodes.ERROR_USER_ALREADY_EXISTS;

			// only administrators can set admin role
			if (!SecurityContext.User.IsInRole(SecurityContext.ROLE_ADMINISTRATOR) &&
				user.Role == UserRole.Administrator)
				user.Role = UserRole.User;

			// check integrity when creating a user account
			if (user.Role == UserRole.User)
				user.EcommerceEnabled = false;

			// place log record
			TaskManager.StartTask("USER", "ADD", user.Username);

			try
			{
				// add user to database
				int userId = DataProvider.AddUser(
					SecurityContext.User.UserId,
					user.OwnerId,
					user.RoleId,
					user.StatusId,
                    user.SubscriberNumber,
                    user.LoginStatusId,
					user.IsDemo,
					user.IsPeer,
					user.Comments,
					user.Username.Trim(),
					CryptoUtils.Encrypt(password),
					user.FirstName,
					user.LastName,
					user.Email,
					user.SecondaryEmail,
					user.Address,
					user.City,
					user.Country,
					user.State,
					user.Zip,
					user.PrimaryPhone,
					user.SecondaryPhone,
					user.Fax,
					user.InstantMessenger,
					user.HtmlMail,
					user.CompanyName,
					user.EcommerceEnabled);

				if (userId == -1)
				{
					TaskManager.WriteWarning("Account with such username already exists");
					return BusinessErrorCodes.ERROR_USER_ALREADY_EXISTS;
				}

                BackgroundTask topTask = TaskManager.TopTask;

			    topTask.ItemId = userId;
                topTask.UpdateParamValue("SendLetter", sendLetter);

                TaskController.UpdateTaskWithParams(topTask);

				return userId;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

        const string userAdditionalParamsTemplate = @"<Params><VLans/></Params>";

        public static void AddUserVLan(int userId, UserVlan vLan)
        {
            UserInfo user = GetUser(userId);
            //
			if (user == null)
                throw new ApplicationException(String.Format("User with UserID={0} not found", userId));

            XDocument doc = XDocument.Parse(user.AdditionalParams ?? userAdditionalParamsTemplate);
            XElement vlans = doc.Root.Element("VLans");
            vlans.Add(new XElement("VLan", new XAttribute("VLanID", vLan.VLanID), new XAttribute("Comment", vLan.Comment)));

            user.AdditionalParams = doc.ToString();
            UpdateUser(user);
        }

        public static void DeleteUserVLan(int userId, ushort vLanId)
        {
            UserInfo user = GetUser(userId);
			//
            if (user == null)
                throw new ApplicationException(String.Format("User with UserID={0} not found", userId));

            XDocument doc = XDocument.Parse(user.AdditionalParams ?? userAdditionalParamsTemplate);
            XElement vlans = doc.Root.Element("VLans");
            if (vlans != null)
                foreach (XElement element in vlans.Elements("VLan"))
                    if (ushort.Parse(element.Attribute("VLanID").Value) == vLanId)
                        element.Remove();

            user.AdditionalParams = doc.ToString();
            UpdateUser(user);
        }

		public static int UpdateUser(UserInfo user)
		{
			return UpdateUser(null, user);
		}

		public static int UpdateUserAsync(string taskId, UserInfo user)
		{
			UserAsyncWorker usersWorker = new UserAsyncWorker();
			usersWorker.ThreadUserId = SecurityContext.User.UserId;
			usersWorker.TaskId = taskId;
			usersWorker.User = user;
			usersWorker.UpdateUserAsync();
			return 0;
		}

		public static int UpdateUser(string taskId, UserInfo user)
		{
			try
			{
				// start task
				TaskManager.StartTask(taskId, "USER", "UPDATE", user.Username, user.UserId);

				// check account
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
				if (accountCheck < 0) return accountCheck;

				UserInfo currentUser = GetUser(user.UserId);

				//prevent downgrade reseller with child accounts to user role
				if (currentUser.RoleId.Equals(2))
				{
					if (user.RoleId.Equals(3))
					{
						// check if user has child accounts (It happens when Reseller has child accounts and admin changes his role to User)
						if (GetUsers(currentUser.UserId, false).Count > 0)
							return BusinessErrorCodes.ERROR_USER_HAS_USERS;
					}
				}


				// only administrators can set admin role
				if (!SecurityContext.User.IsInRole(SecurityContext.ROLE_ADMINISTRATOR) &&
					user.Role == UserRole.Administrator)
					user.Role = UserRole.User;

				// check integrity when creating a user account
				if (user.Role == UserRole.User)
					user.EcommerceEnabled = false;

				//// task progress
				//int maxSteps = 100;
				//TaskManager.IndicatorMaximum = maxSteps;
				//for (int i = 0; i < maxSteps; i++)
				//{
				//    TaskManager.Write(String.Format("Step {0} of {1}", i, maxSteps));
				//    TaskManager.IndicatorCurrent = i;
				//    System.Threading.Thread.Sleep(1000);
				//}

				DataProvider.UpdateUser(
					SecurityContext.User.UserId,
					user.UserId,
					user.RoleId,
					user.StatusId,
                    user.SubscriberNumber,
                    user.LoginStatusId,
					user.IsDemo,
					user.IsPeer,
					user.Comments,
					user.FirstName,
					user.LastName,
					user.Email,
					user.SecondaryEmail,
					user.Address,
					user.City,
					user.Country,
					user.State,
					user.Zip,
					user.PrimaryPhone,
					user.SecondaryPhone,
					user.Fax,
					user.InstantMessenger,
					user.HtmlMail,
					user.CompanyName,
					user.EcommerceEnabled,
                    user.AdditionalParams);

				return 0;
			}
			catch (System.Threading.ThreadAbortException ex)
			{
				TaskManager.WriteError(ex, "The process has been terminated by the user.");
				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int DeleteUser(int userId)
		{
			return DeleteUser(null, userId);
		}

		public static int DeleteUser(string taskId, int userId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// check if user has child accounts
			if (GetUsers(userId, false).Count > 0)
				return BusinessErrorCodes.ERROR_USER_HAS_USERS;

			UserAsyncWorker userWorker = new UserAsyncWorker();
			userWorker.ThreadUserId = SecurityContext.User.UserId;
			userWorker.UserId = userId;
			userWorker.TaskId = taskId;
			userWorker.DeleteUserAsync();

			return 0;
		}

		public static int ChangeUserPassword(int userId, string password)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// get user details
			UserInfo user = GetUserInternally(userId);

			// place log record
            TaskManager.StartTask("USER", "CHANGE_PASSWORD", user.Username, user.UserId);

            try
			{

				DataProvider.ChangeUserPassword(SecurityContext.User.UserId, userId,
					CryptoUtils.Encrypt(password));

				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int ChangeUserStatus(int userId, UserStatus status)
		{
			return ChangeUserStatus(null, userId, status);
		}

		public static int ChangeUserStatus(string taskId, int userId, UserStatus status)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			int result = 0;

			// get user details
			UserInfo user = GetUserInternally(userId);

			// place log record
			TaskManager.StartTask(taskId, "USER", "CHANGE_STATUS", user.Username, user.UserId);

            // update user packages
            List<PackageInfo> packages = new List<PackageInfo>();

            // Add the users package(s)
            packages.AddRange(PackageController.GetPackages(userId));

			try
			{

				// change this account
				result = ChangeUserStatusInternal(userId, status);
				if (result < 0)
					return result;

				// change peer accounts
				List<UserInfo> peers = GetUserPeers(userId);
				foreach (UserInfo peer in peers)
				{
					result = ChangeUserStatusInternal(peer.UserId, status);
					if (result < 0)
						return result;
				}

				// change child accounts
				List<UserInfo> children = GetUsers(userId, true);
				foreach (UserInfo child in children)
				{
                    // Add the child users packages
                    packages.AddRange(PackageController.GetPackages(child.UserId));

                    // change child user peers
                    List<UserInfo> childPeers = GetUserPeers(child.UserId);
                    foreach (UserInfo peer in childPeers)
                    {
                        result = ChangeUserStatusInternal(peer.UserId, status);
                        if (result < 0)
                            return result;
                    }

                    // change child account
                    result = ChangeUserStatusInternal(child.UserId, status);
                    if (result < 0)
                        return result;
				}

				PackageStatus packageStatus = PackageStatus.Active;
				switch (status)
				{
					case UserStatus.Active: packageStatus = PackageStatus.Active; break;
					case UserStatus.Cancelled: packageStatus = PackageStatus.Cancelled; break;
					case UserStatus.Pending: packageStatus = PackageStatus.New; break;
					case UserStatus.Suspended: packageStatus = PackageStatus.Suspended; break;
				}

				// change packages state
				result = PackageController.ChangePackagesStatus(packages, packageStatus, true);
				if (result < 0)
					return result;

				return 0;

			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		private static int ChangeUserStatusInternal(int userId, UserStatus status)
		{
			// get user details
			UserInfo user = GetUser(userId);
			if (user != null && user.Status != status)
			{
				// change status
				user.Status = status;

				// save user
				UpdateUser(user);
			}

			return 0;
		}

		#region User Settings
		public static UserSettings GetUserSettings(int userId, string settingsName)
		{
			IDataReader reader = DataProvider.GetUserSettings(
				SecurityContext.User.UserId, userId, settingsName);

			UserSettings settings = new UserSettings();
			settings.UserId = userId;
			settings.SettingsName = settingsName;

			while (reader.Read())
			{
				settings.UserId = (int)reader["UserID"];
				settings[(string)reader["PropertyName"]] = (string)reader["PropertyValue"];
			}
			reader.Close();

			return settings;
		}

		public static int UpdateUserSettings(UserSettings settings)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// get user details
			UserInfo user = GetUserInternally(settings.UserId);

			// place log record
            TaskManager.StartTask("USER", "UPDATE_SETTINGS", user.Username, user.UserId);

			try
			{
				// build xml
				XmlDocument doc = new XmlDocument();
				XmlElement nodeProps = doc.CreateElement("properties");
				if (settings.SettingsArray != null)
				{
					foreach (string[] pair in settings.SettingsArray)
					{
						XmlElement nodeProp = doc.CreateElement("property");
						nodeProp.SetAttribute("name", pair[0]);
						nodeProp.SetAttribute("value", pair[1]);
						nodeProps.AppendChild(nodeProp);
					}
				}

				string xml = nodeProps.OuterXml;

				// update settings
				DataProvider.UpdateUserSettings(SecurityContext.User.UserId,
					settings.UserId, settings.SettingsName, xml);

				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}



		#endregion

		#region Themes

		public static DataSet GetUserThemeSettings(int userId)
		{
			return DataProvider.GetUserThemeSettings(SecurityContext.User.UserId, userId);
		}

		public static void UpdateUserThemeSetting(int userId, string PropertyName, string PropertyValue)
		{
			DataProvider.UpdateUserThemeSetting(SecurityContext.User.UserId, userId, PropertyName, PropertyValue);
		}

		public static void DeleteUserThemeSetting(int userId, string PropertyName)
		{
			DataProvider.DeleteUserThemeSetting(SecurityContext.User.UserId, userId, PropertyName);
		}

		#endregion
	}
}
