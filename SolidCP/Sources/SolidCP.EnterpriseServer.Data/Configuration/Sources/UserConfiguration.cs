// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using SolidCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class UserConfiguration: Extensions.EntityTypeConfiguration<User>
{
    public UserConfiguration(): base() { }
    public UserConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        Property(e => e.HtmlMail).HasDefaultValue(true);

        HasOne(d => d.Owner).WithMany(p => p.InverseOwner).HasConstraintName("FK_Users_Users");

#region Seed Data
        HasData(
            new User() { UserId = 1, AdditionalParams = null, Address = "", Changed = DateTime.Parse("2010-07-16T12:53:02.4530000"), City = "", Comments = "",
                CompanyName = null, Country = "", Created = DateTime.Parse("2010-07-16T12:53:02.4530000"), EcommerceEnabled = true, Email = "serveradmin@myhosting.com", FailedLogins = null,
                Fax = "", FirstName = "Enterprise", HtmlMail = true, InstantMessenger = "", IsDemo = false, IsPeer = false,
                LastName = "Administrator", LoginStatusId = null, MfaMode = 0, OneTimePasswordState = null, OwnerId = null, Password = "",
                PinSecret = null, PrimaryPhone = "", RoleId = 1, SecondaryEmail = "", SecondaryPhone = "", State = "",
                StatusId = 1, SubscriberNumber = null, Username = "serveradmin", Zip = "" }
        );
#endregion

    }
#endif
}
