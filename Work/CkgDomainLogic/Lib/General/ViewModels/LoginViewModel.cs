﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Xml.Serialization;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using WebTools.Services;

namespace CkgDomainLogic.General.ViewModels
{
    public class LoginViewModel : CkgCommonViewModel 
    {
        [XmlIgnore]
        public ISecurityService SecurityService { get { return CacheGet<ISecurityService>(); } }

        [XmlIgnore]
        public ILocalizationService LocalizationService { get { return CacheGet<ILocalizationService>(); } }

        private int _passwordRuleCount;
        public int PasswordRuleCount
        {
            get { return _passwordRuleCount; }
        }

        private string _sslCertificateHtml;
        public string SslCertificateHtml
        {
            get
            {
                if (String.IsNullOrEmpty(_sslCertificateHtml))
                    _sslCertificateHtml = GeneralTools.Services.GeneralConfiguration.GetConfigValue("Login", "SecurityCertificate");

                return _sslCertificateHtml;
            }
        }

        public LoginModel LoginModel { get { return PropertyCacheGet(() => new LoginModel()); } set { PropertyCacheSet(value); } }

        public ChangePasswordModel ChangePasswordModel { get { return PropertyCacheGet(() => new ChangePasswordModel()); } set { PropertyCacheSet(value); } }

        public User TmpUser { get; set; }

        public Customer TmpCustomer { get; set; }


        public void TryLogonUser(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
            LogonContext.TryLogonUser(loginModel, addModelError);
        }

        public string TryGetEmailAddressFromUsername(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
            return LogonContext.TryGetEmailAddressFromUsername(loginModel, addModelError);
        }

        public void CheckIfPasswordResetAllowed(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
            LogonContext.CheckIfPasswordResetAllowed(loginModel, addModelError);
        }

        public void ValidatePasswordModelAgainstRules(Action<string, string> addModelError)
        {
            List<string> localizedPasswordValidationErrorMessages;
            List<string> localizedPasswordRuleMessages;
            ValidatePasswordAgainstRules(ChangePasswordModel.Password, out localizedPasswordValidationErrorMessages, out localizedPasswordRuleMessages);
            if (localizedPasswordValidationErrorMessages.Any())
                addModelError("Password", string.Join("; ", localizedPasswordValidationErrorMessages));
        }

        public void ValidatePasswordAgainstRules(string password, out List<string> localizedPasswordValidationErrorMessages, out List<string> localizedPasswordRuleMessages)
        {
            SecurityService.ValidatePassword(password, GetPasswordSecurityRuleDataProvider(ChangePasswordModel.UserName), LocalizationService, out localizedPasswordValidationErrorMessages, out localizedPasswordRuleMessages, out _passwordRuleCount);
        }

        private IPasswordSecurityRuleDataProvider GetPasswordSecurityRuleDataProvider(string userName)
        {
            var provider = LogonContext.Customer;
            if (provider != null)
                return provider;
            
            var lc = LogonContext;
            lc.LogonUser(userName);
            provider = lc.Customer;

            return provider;
        }

        public bool CacheUserAndCustomerFromConfirmationToken(string confirmation)
        {
            var user = LogonContext.TryGetUserFromPasswordToken(confirmation, AppSettings.TokenExpirationMinutes);
            if (user == null)
                return false;

            return CacheUserAndCustomer(user);
        }

        public bool CacheUserAndCustomerFromUserName(string userName)
        {
            var user = LogonContext.TryGetUserFromUserName(userName);
            if (user == null)
                return false;

            return CacheUserAndCustomer(user);
        }

        private bool CacheUserAndCustomer(User user)
        {
            var customer = LogonContext.TryGetCustomerFromUserName(user.Username);
            if (customer == null)
                return false;

            TmpUser = user;
            TmpCustomer = customer;

            ChangePasswordModel.UserName = user.Username;
            ChangePasswordModel.UserSalutation = user.UserSalutation;

            return true;
        }

        public string TryGetPasswordResetCustomerAdminInfo(string userName)
        {
            var customer = GetPasswordSecurityRuleDataProvider(userName) as Customer;
            if (customer != null && customer.PwdDontSendEmail && customer.SelfAdministrationContact.IsNotNullOrEmpty())
                return LoginUserMessage.ConvertMessage(customer.SelfAdministrationContact);

            return "";
        }

        public void TrySendPasswordResetEmail(string userName, string userEmail, string url, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
            try
            {
                var confirmationToken = UserSecurityService.GenerateToken(userName);
                LogonContext.StorePasswordRequestKeyToUser(userName, confirmationToken);
                
                var user = LogonContext.TryGetUserFromUserName(userName);
                var userSalutation = user == null ? userName : user.UserSalutation;

                var confirmationUrl = url.ToLower().Replace("loginform", "changepassword") + "?confirmation=" +
                                      HttpUtility.UrlEncode(confirmationToken);
                var confirmationLink = string.Format("<a href=\"{0}\">{0}</a>", confirmationUrl);

                var subject = string.Format(Localize.LoginPasswordResetEmailSubject, AppSettings.AppOwnerFullName);
                var body = string.Format("{0}<br /><br />{1}<br /><br /><strong>{2}</strong><br /><br />{3}",
                                         string.Format(Localize.LoginPasswordResetEmailBody, userSalutation),
                                         confirmationLink,
                                         string.Format(Localize.LoginPasswordResetLinkExpirationHint, DateTime.Now.AddMinutes(AppSettings.TokenExpirationMinutes).ToString("dd.MM.yyyy, HH:mm")),
                                         string.Format(Localize.LoginPasswordResetEmailFooter, AppSettings.AppOwnerFullName)
                                        );

                AppSettings.MailService.SendMail(userEmail, subject, body);
            }
            catch
            {
                addModelError(m => m.EmailForPasswordReset, Localize.EmailSentError);
            }
        }
    }
}
