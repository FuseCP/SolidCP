// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities.Sources;

#if NetCore
[Index("Username", Name = "IX_Users_Username", IsUnique = true)]
[Index("OwnerId", Name = "UsersIdx_OwnerID")]
#endif
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [Column("OwnerID")]
    public int? OwnerId { get; set; }

    [Column("RoleID")]
    public int RoleId { get; set; }

    [Column("StatusID")]
    public int StatusId { get; set; }

    public bool IsDemo { get; set; }

    public bool IsPeer { get; set; }

    [StringLength(50)]
    public string Username { get; set; }

    [StringLength(200)]
    public string Password { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(255)]
    public string Email { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Created { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Changed { get; set; }

    [Column(TypeName = "ntext")]
    public string Comments { get; set; }

    [StringLength(255)]
    public string SecondaryEmail { get; set; }

    [StringLength(200)]
    public string Address { get; set; }

    [StringLength(50)]
    public string City { get; set; }

    [StringLength(50)]
    public string State { get; set; }

    [StringLength(50)]
    public string Country { get; set; }

    [StringLength(20)]
#if NetCore
    [Unicode(false)]
#endif
    public string Zip { get; set; }

    [StringLength(30)]
#if NetCore
    [Unicode(false)]
#endif
    public string PrimaryPhone { get; set; }

    [StringLength(30)]
#if NetCore
    [Unicode(false)]
#endif
    public string SecondaryPhone { get; set; }

    [StringLength(30)]
#if NetCore
    [Unicode(false)]
#endif
    public string Fax { get; set; }

    [StringLength(100)]
#if NetCore
    [Unicode(false)]
#endif
    public string InstantMessenger { get; set; }

    public bool? HtmlMail { get; set; }

    [StringLength(100)]
    public string CompanyName { get; set; }

    public bool? EcommerceEnabled { get; set; }

    public string AdditionalParams { get; set; }

    public int? LoginStatusId { get; set; }

    public int? FailedLogins { get; set; }

    [StringLength(32)]
    public string SubscriberNumber { get; set; }

    public int? OneTimePasswordState { get; set; }

    public int MfaMode { get; set; }

    [StringLength(255)]
    public string PinSecret { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Comment> CommentsNavigation { get; set; } = new List<Comment>();

    [InverseProperty("User")]
    public virtual ICollection<HostingPlan> HostingPlans { get; set; } = new List<HostingPlan>();

    [InverseProperty("Owner")]
    public virtual ICollection<User> InverseOwner { get; set; } = new List<User>();

    [ForeignKey("OwnerId")]
    [InverseProperty("InverseOwner")]
    public virtual User Owner { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    [InverseProperty("User")]
    public virtual ICollection<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
}
#endif