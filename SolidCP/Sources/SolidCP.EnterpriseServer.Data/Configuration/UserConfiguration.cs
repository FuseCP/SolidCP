using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
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

#if NetCore
        Property(e => e.HtmlMail).HasDefaultValue(true);

        HasOne(d => d.Owner).WithMany(p => p.InverseOwner).HasConstraintName("FK_Users_Users");
#else
        HasOptional(d => d.Owner).WithMany(p => p.InverseOwner);
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
