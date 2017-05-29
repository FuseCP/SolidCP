using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.EnterpriseServer
{
    public class UserPasswordExpirationNotificationTask : SchedulerTask
    {
        // Input parameters:
        private static readonly string DaysBeforeNotify = "DAYS_BEFORE_EXPIRATION";

        public override void DoWork()
        {
            BackgroundTask topTask = TaskManager.TopTask;

            int daysBeforeNotify;

            // check input parameters
            if (!int.TryParse((string)topTask.GetParamValue(DaysBeforeNotify), out daysBeforeNotify))
            {
                TaskManager.WriteWarning("Specify 'Notify before (days)' task parameter");
                return;
            }

            OrganizationController.DeleteAllExpiredTokens();

            var owner = UserController.GetUser(topTask.EffectiveUserId);

            var packages = PackageController.GetMyPackages(topTask.EffectiveUserId);

            foreach (var package in packages)
            {
                var organizations = ExchangeServerController.GetExchangeOrganizations(package.PackageId, true);
                
                foreach (var organization in organizations)
                {
                    var usersWithExpiredPasswords = OrganizationController.GetOrganizationUsersWithExpiredPassword(organization.Id, daysBeforeNotify);

                    var generalSettings = OrganizationController.GetOrganizationGeneralSettings(organization.Id);

                    var logoUrl = generalSettings != null ? generalSettings.OrganizationLogoUrl : string.Empty;

                    foreach (var user in usersWithExpiredPasswords)
                    {
                        user.ItemId = organization.Id;

                        if (string.IsNullOrEmpty(user.PrimaryEmailAddress))
                        {
                            TaskManager.WriteWarning(string.Format("Unable to send email to {0} user (organization: {1}), user primary email address is not set.", user.DisplayName, organization.OrganizationId));
                            continue;
                        }

                        OrganizationController.SendUserExpirationPasswordEmail(owner, user, "Scheduler Password Expiration Notification", user.PrimaryEmailAddress, logoUrl);
                    }
                }
            }
        }
    }
}