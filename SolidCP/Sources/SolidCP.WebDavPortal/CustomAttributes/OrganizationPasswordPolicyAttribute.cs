using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SolidCP.Providers.HostedSolution;
using SolidCP.WebDav.Core;
using SolidCP.WebDav.Core.Config;

namespace SolidCP.WebDavPortal.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class OrganizationPasswordPolicyAttribute : ValidationAttribute, IClientValidatable
    {
        public int ItemId { get; private set; }

        public OrganizationPasswordPolicyAttribute()
        {
            if (ScpContext.User != null)
            {
                ItemId = ScpContext.User.ItemId;
            }
            else if (HttpContext.Current != null && HttpContext.Current.Session[WebDavAppConfigManager.Instance.SessionKeys.ItemId] != null)
            {
                ItemId = (int)HttpContext.Current.Session[WebDavAppConfigManager.Instance.SessionKeys.ItemId];
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var resultMessages = new List<string>();

                var settings = ScpContext.Services.Organizations.GetOrganizationPasswordSettings(ItemId);

                if (settings != null)
                {
                    var valueString = value.ToString();

                    if (valueString.Length < settings.MinimumLength)
                    {
                        resultMessages.Add(string.Format(Resources.Messages.PasswordMinLengthFormat,
                            settings.MinimumLength));
                    }

                    if (valueString.Length > settings.MaximumLength)
                    {
                        resultMessages.Add(string.Format(Resources.Messages.PasswordMaxLengthFormat,
                            settings.MaximumLength));
                    }

                    if (settings.PasswordComplexityEnabled)
                    {
                        var numbersCount = valueString.Count(Char.IsDigit);
                        var upperLetterCount = valueString.Count(Char.IsUpper);
                        var symbolsCount = Regex.Matches(valueString, @"[~!@#$%^&*_\-+'\|\\(){}\[\]:;\""'<>,.?/]").Count;

                        if (upperLetterCount < settings.UppercaseLettersCount)
                        {
                            resultMessages.Add(string.Format(Resources.Messages.PasswordUppercaseCountFormat,
                                settings.UppercaseLettersCount));
                        }

                        if (numbersCount < settings.NumbersCount)
                        {
                            resultMessages.Add(string.Format(Resources.Messages.PasswordNumbersCountFormat,
                                settings.NumbersCount));
                        }

                        if (symbolsCount < settings.SymbolsCount)
                        {
                            resultMessages.Add(string.Format(Resources.Messages.PasswordSymbolsCountFormat,
                                settings.SymbolsCount));
                        }
                    }

                }

                return resultMessages.Any()?  new ValidationResult(string.Join("<br>", resultMessages)) : ValidationResult.Success;
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var settings = ScpContext.Services.Organizations.GetOrganizationPasswordSettings(ItemId);

            var rule = new ModelClientValidationRule();

            rule.ErrorMessage = string.Format(Resources.Messages.PasswordMinLengthFormat, settings.MinimumLength);
            rule.ValidationParameters.Add("count", settings.MinimumLength);
            rule.ValidationType = "minimumlength";

            yield return rule;

            rule = new ModelClientValidationRule();

            rule.ErrorMessage = string.Format(Resources.Messages.PasswordMaxLengthFormat, settings.MaximumLength);
            rule.ValidationParameters.Add("count", settings.MaximumLength);
            rule.ValidationType = "maximumlength";

            yield return rule;

            if (settings.PasswordComplexityEnabled)
            {
                rule = new ModelClientValidationRule();

                rule.ErrorMessage = string.Format(Resources.Messages.PasswordUppercaseCountFormat, settings.UppercaseLettersCount);
                rule.ValidationParameters.Add("count", settings.UppercaseLettersCount);
                rule.ValidationType = "uppercasecount";

                yield return rule;

                rule = new ModelClientValidationRule();

                rule.ErrorMessage = string.Format(Resources.Messages.PasswordNumbersCountFormat, settings.NumbersCount);
                rule.ValidationParameters.Add("count", settings.NumbersCount);
                rule.ValidationType = "numberscount";

                yield return rule;

                rule = new ModelClientValidationRule();

                rule.ErrorMessage = string.Format(Resources.Messages.PasswordSymbolsCountFormat, settings.SymbolsCount);
                rule.ValidationParameters.Add("count", settings.SymbolsCount);
                rule.ValidationType = "symbolscount";

                yield return rule;
            }
        }

    }
}