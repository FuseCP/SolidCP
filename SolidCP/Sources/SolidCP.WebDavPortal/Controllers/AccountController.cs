﻿using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using SolidCP.Providers.HostedSolution;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Security.Authentication;
using SolidCP.WebDav.Core.Security.Cryptography;
using SolidCP.WebDav.Core.Scp.Framework;
using SolidCP.WebDavPortal.CustomAttributes;
using SolidCP.WebDavPortal.Models;
using SolidCP.WebDavPortal.Models.Account;
using SolidCP.WebDavPortal.Models.Account.Enums;
using SolidCP.WebDavPortal.Models.Common;
using SolidCP.WebDavPortal.Models.Common.EditorTemplates;
using SolidCP.WebDavPortal.Models.Common.Enums;
using SolidCP.WebDavPortal.UI.Routes;
using SolidCP.WebDav.Core.Interfaces.Security;
using SolidCP.WebDav.Core;
using SolidCP.Server.Utils;

namespace SolidCP.WebDavPortal.Controllers
{
    [LdapAuthorization]
    public class AccountController : BaseController
    {
        private readonly ICryptography _cryptography;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISmsAuthenticationService _smsAuthService;

        public AccountController(ICryptography cryptography, IAuthenticationService authenticationService, ISmsAuthenticationService smsAuthService)
        {
            _cryptography = cryptography;
            _authenticationService = authenticationService;
            _smsAuthService = smsAuthService;        
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            Log.WriteStart("Login");

            if (ScpContext.User != null && ScpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToRoute(FileSystemRouteNames.ShowContentPath, new { org = ScpContext.User.OrganizationId });
            }

            var model = new AccountModel();

            var settings = ScpContext.Services.Organizations.GetWebDavSystemSettings();

            if (settings != null)
            {
                model.PasswordResetEnabled = settings.GetValueOrDefault(EnterpriseServer.SystemSettings.WEBDAV_PASSWORD_RESET_ENABLED_KEY, false);

            }

            Log.WriteEnd("Login");

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(AccountModel model)
        {
            Log.WriteStart("Login with Model");

            var user = _authenticationService.LogIn(model.Login, model.Password);
            
            ViewBag.LdapIsAuthentication = user != null;

            if (user != null && user.Identity.IsAuthenticated)
            {
                _authenticationService.CreateAuthenticationTicket(user);

                return RedirectToRoute(FileSystemRouteNames.ShowContentPath, new { org = ScpContext.User.OrganizationId });
            }

            Log.WriteEnd("Login with Model");

            return View(new AccountModel { LdapError = "The user name or password is incorrect" });
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _authenticationService.LogOut();

            Session.Clear();

            return RedirectToRoute(AccountRouteNames.Login);
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            var model = GetUserProfileModel(ScpContext.User.ItemId, ScpContext.User.AccountId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(UserProfile model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int result = UpdateUserProfile(ScpContext.User.ItemId, ScpContext.User.AccountId, model);

            AddMessage(MessageType.Success, Resources.UI.UserProfileSuccessfullyUpdated);

            return View(model);
        }

        public JsonResult PhoneNumberIsAvailible()
        {
            var value = Request.QueryString.AllKeys.Any() ? Request.QueryString.Get(0) :string.Empty;

            var result = !ScpContext.Services.Organizations.CheckPhoneNumberIsInUse(ScpContext.User.ItemId,
                    value, ScpContext.User.Login);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PasswordChange()
        {
            var model = new PasswordChangeModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult PasswordChange(PasswordChangeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_authenticationService.ValidateAuthenticationData(ScpContext.User.Login, model.OldPassword) == false)
            {
                AddMessage(MessageType.Error, Resources.Messages.OldPasswordIsNotCorrect);

                return View(model);
            }

            ScpContext.Services.Organizations.SetUserPassword(
                    ScpContext.User.ItemId, ScpContext.User.AccountId,
                    model.PasswordEditor.NewPassword);

            var user = _authenticationService.LogIn(ScpContext.User.Login, model.PasswordEditor.NewPassword);

            _authenticationService.CreateAuthenticationTicket(user);

            AddMessage(MessageType.Success, Resources.Messages.PasswordSuccessfullyChanged);

            return RedirectToRoute(AccountRouteNames.UserProfile);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordResetLogin()
        {
            var model = new PasswordResetLoginModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PasswordResetLogin(PasswordResetLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var exchangeAccount = ScpContext.Services.ExchangeServer.GetAccountByAccountNameWithoutItemId(model.Email);

            if (exchangeAccount == null)
            {
                AddMessage(MessageType.Error, Resources.Messages.AccountNotFound);

                return View(model);
            }

            var tokenEntity = ScpContext.Services.Organizations.CreatePasswordResetAccessToken(exchangeAccount.ItemId, exchangeAccount.AccountId);

            return RedirectToRoute(AccountRouteNames.PasswordResetPincodeSendOptions, new {token = tokenEntity.AccessTokenGuid.ToString("N")});
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordResetPincodeSendOptions(Guid token)
        {
            var accessToken = ScpContext.Services.Organizations.GetPasswordresetAccessToken(token);

            if (accessToken == null)
            {
                AddMessage(MessageType.Error, Resources.Messages.IncorrectPasswordResetUrl);

                return RedirectToRoute(AccountRouteNames.PasswordResetLogin);
            }

            var user = ScpContext.Services.Organizations.GetUserGeneralSettings(accessToken.ItemId, accessToken.AccountId);

            var settings = ScpContext.Services.System.GetSystemSettingsActive(EnterpriseServer.SystemSettings.TWILIO_SETTINGS, false);

            bool twilioEnabled = settings != null
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(EnterpriseServer.SystemSettings.TWILIO_ACCOUNTSID_KEY, string.Empty))
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(EnterpriseServer.SystemSettings.TWILIO_AUTHTOKEN_KEY, string.Empty))
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(EnterpriseServer.SystemSettings.TWILIO_PHONEFROM_KEY, string.Empty));

            if (string.IsNullOrEmpty(user.MobilePhone) || twilioEnabled == false)
            {
                var result = ScpContext.Services.Organizations.SendResetUserPasswordPincodeEmail(accessToken.AccessTokenGuid, user.PrimaryEmailAddress);

                if (result.IsSuccess)
                {
                    AddMessage(MessageType.Success, Resources.Messages.PincodeEmailWasSent);
                }
                else
                {
                    AddMessage(MessageType.Error, Resources.Messages.PincodeEmailWasNotSent);
                }

                return RedirectToRoute(AccountRouteNames.PasswordResetPincode);
            }

            var model = new PasswordResetPincodeSendOptionsModel();

            model.MobileNumber = user.MobilePhone;
            model.Email = user.PrimaryEmailAddress;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PasswordResetPincodeSendOptions(Guid token, PasswordResetPincodeSendOptionsModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var accessToken = ScpContext.Services.Organizations.GetPasswordresetAccessToken(token);

            if (accessToken == null)
            {
                AddMessage(MessageType.Error, Resources.Messages.IncorrectPasswordResetUrl);

                return RedirectToRoute(AccountRouteNames.PasswordResetLogin);
            }

            var user = ScpContext.Services.Organizations.GetUserGeneralSettings(accessToken.ItemId, accessToken.AccountId);

            switch (model.Method)
            {
                case PincodeSendMethod.Mobile:
                {
                    var result = ScpContext.Services.Organizations.SendResetUserPasswordPincodeSms(accessToken.AccessTokenGuid, user.MobilePhone);

                    if (result.IsSuccess)
                    {
                        AddMessage(MessageType.Success, Resources.Messages.SmsWasSent);
                    }
                    else
                    {
                        AddMessage(MessageType.Error, Resources.Messages.SmsWasNotSent);

                        return RedirectToRoute(AccountRouteNames.PasswordResetPincodeSendOptions);
                    }

                    break;
                }
                case PincodeSendMethod.Email:
                {
                    var result = ScpContext.Services.Organizations.SendResetUserPasswordPincodeEmail(accessToken.AccessTokenGuid, user.PrimaryEmailAddress);

                    if (result.IsSuccess)
                    {
                        AddMessage(MessageType.Success, Resources.Messages.PincodeEmailWasSent);
                    }
                    else
                    {
                        AddMessage(MessageType.Error, Resources.Messages.PincodeEmailWasNotSent);

                        return RedirectToRoute(AccountRouteNames.PasswordResetPincodeSendOptions);
                    }

                    break;
                }
                
            }

            return RedirectToRoute(AccountRouteNames.PasswordResetPincode);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordResetPincode(Guid token)
        {
            var model = new PasswordResetPincodeModel();

            var accessToken = ScpContext.Services.Organizations.GetPasswordresetAccessToken(token);

            model.IsTokenExist = accessToken != null;

            if (model.IsTokenExist == false)
            {
                AddMessage(MessageType.Error, Resources.Messages.IncorrectPasswordResetUrl);

                return RedirectToRoute(AccountRouteNames.PasswordResetLogin);
            }


            if (accessToken != null && accessToken.IsSmsSent == false)
            {
                return RedirectToRoute(AccountRouteNames.PasswordResetPincodeSendOptions);
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PasswordResetPincode(Guid token, PasswordResetPincodeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_smsAuthService.VerifyResponse(token, model.Sms))
            {
                var tokenEntity = ScpContext.Services.Organizations.GetPasswordresetAccessToken(token);

                Session[WebDavAppConfigManager.Instance.SessionKeys.PasswordResetSmsKey] = model.Sms;
                Session[WebDavAppConfigManager.Instance.SessionKeys.ItemId] = tokenEntity.ItemId;

                return RedirectToRoute(AccountRouteNames.PasswordResetFinalStep);
            }

            AddMessage(MessageType.Error, Resources.Messages.IncorrectSmsResponse);

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordResetFinalStep(Guid token, string pincode)
        {
            var result = VerifyPincode(token, pincode);

            if (result != null)
            {
                return result;
            }

            var tokenEntity = ScpContext.Services.Organizations.GetPasswordresetAccessToken(token);
            var account = ScpContext.Services.Organizations.GetUserGeneralSettings(tokenEntity.ItemId,
                tokenEntity.AccountId);

            var model = new PasswordResetFinalStepModel();

            model.PasswordEditor.Settings = ScpContext.Services.Organizations.GetOrganizationPasswordSettings(tokenEntity.ItemId);
            model.Login = account.UserPrincipalName;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PasswordResetFinalStep(Guid token, string pincode, PasswordResetFinalStepModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = VerifyPincode(token, pincode);

            if (result != null)
            {
                return result;
            }

            var tokenEntity = ScpContext.Services.Organizations.GetPasswordresetAccessToken(token);

            ScpContext.Services.Organizations.SetUserPassword(
                    tokenEntity.ItemId, tokenEntity.AccountId,
                    model.PasswordEditor.NewPassword);

            ScpContext.Services.Organizations.DeletePasswordresetAccessToken(token);

            return RedirectToRoute(AccountRouteNames.PasswordResetSuccess);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordSuccessfullyChanged()
        {
            return View();
        }

        #region Helpers

        /// <summary>
        /// Verify pincode, if it's absent - verifying pincode from session 
        /// </summary>
        /// <param name="token">Password reset token</param>
        /// <param name="pincode">Pincode to verify if session pincode is absent</param>
        private ActionResult VerifyPincode(Guid token, string pincode)
        {
            var smsResponse = Session[WebDavAppConfigManager.Instance.SessionKeys.PasswordResetSmsKey] as string;

            if (string.IsNullOrEmpty(pincode) == false)
            {
                smsResponse = pincode;
            }

            if (_smsAuthService.VerifyResponse(token, smsResponse) == false)
            {
                AddMessage(MessageType.Error, Resources.Messages.IncorrectSmsResponse);

                return RedirectToRoute(AccountRouteNames.PasswordResetPincode); //todo
            }

            var tokenEntity = ScpContext.Services.Organizations.GetPasswordresetAccessToken(token);

            Session[WebDavAppConfigManager.Instance.SessionKeys.ItemId] = tokenEntity.ItemId;

            return null;
        }

        private UserProfile GetUserProfileModel(int itemId, int accountId)
        {
            var user = ScpContext.Services.Organizations.GetUserGeneralSettingsWithExtraData(itemId, accountId);

            return Mapper.Map<OrganizationUser, UserProfile>(user);
        }

        private int UpdateUserProfile(int itemId, int accountId, UserProfile model)
        {
            var user = ScpContext.Services.Organizations.GetUserGeneralSettings(itemId, accountId);

            return ScpContext.Services.Organizations.SetUserGeneralSettings(
                itemId, accountId,
                model.DisplayName,
                string.Empty,
                false,
                user.Disabled,
                user.Locked,

                model.FirstName,
                model.Initials,
                model.LastName,

                model.Address,
                model.City,
                model.State,
                model.Zip,
                model.Country,

                user.JobTitle,
                user.Company,
                user.Department,
                user.Office,
                user.Manager == null ? null : user.Manager.AccountName,

                model.BusinessPhone,
                model.Fax,
                model.HomePhone,
                model.MobilePhone,
                model.Pager,
                model.WebPage,
                model.Notes,
                model.ExternalEmail,
                user.SubscriberNumber,
                user.LevelId,
                user.IsVIP,
                user.UserMustChangePassword);
        }

        #endregion

    }
}