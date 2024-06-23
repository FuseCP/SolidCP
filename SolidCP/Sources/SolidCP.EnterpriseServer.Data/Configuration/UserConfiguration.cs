using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class UserConfiguration: EntityTypeConfiguration<User>
{
#if NetCore || NetFX
    public override void Configure() {

		Property(e => e.Zip).IsUnicode(false);
		Property(e => e.PrimaryPhone).IsUnicode(false);
		Property(e => e.SecondaryPhone).IsUnicode(false);
		Property(e => e.Fax).IsUnicode(false);
		Property(e => e.InstantMessenger).IsUnicode(false);

#if NetCore
        Property(e => e.HtmlMail).HasDefaultValue(true);

        HasOne(d => d.Owner).WithMany(p => p.ChildUsers).HasConstraintName("FK_Users_Users");
#else
        HasOptional(d => d.Owner).WithMany(p => p.ChildUsers);
#endif

		#region Seed Data
		HasData(() => new User[] {
			new User() { UserId = 1, Address = "", Changed = DateTime.Parse("2010-07-16T12:53:02.4530000"), City = "", Comments = "", Country = "",
				Created = DateTime.Parse("2010-07-16T12:53:02.4530000"), EcommerceEnabled = true, Email = "serveradmin@myhosting.com", Fax = "", FirstName = "Enterprise", HtmlMail = true,
				InstantMessenger = "", LastName = "Administrator", Password = "", PrimaryPhone = "", RoleId = 1, SecondaryEmail = "",
				SecondaryPhone = "", State = "", StatusId = 1, Username = "serveradmin", Zip = "" }
		});
		#endregion
	}
#endif
}
