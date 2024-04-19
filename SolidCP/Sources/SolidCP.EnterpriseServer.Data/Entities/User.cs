// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class User
{
    public int UserId { get; set; }

    public int? OwnerId { get; set; }

    public int RoleId { get; set; }

    public int StatusId { get; set; }

    public bool IsDemo { get; set; }

    public bool IsPeer { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Changed { get; set; }

    public string Comments { get; set; }

    public string SecondaryEmail { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Country { get; set; }

    public string Zip { get; set; }

    public string PrimaryPhone { get; set; }

    public string SecondaryPhone { get; set; }

    public string Fax { get; set; }

    public string InstantMessenger { get; set; }

    public bool? HtmlMail { get; set; }

    public string CompanyName { get; set; }

    public bool? EcommerceEnabled { get; set; }

    public string AdditionalParams { get; set; }

    public int? LoginStatusId { get; set; }

    public int? FailedLogins { get; set; }

    public string SubscriberNumber { get; set; }

    public int? OneTimePasswordState { get; set; }

    public int MfaMode { get; set; }

    public string PinSecret { get; set; }

    public virtual ICollection<Comment> CommentsNavigation { get; set; } = new List<Comment>();

    public virtual ICollection<HostingPlan> HostingPlans { get; set; } = new List<HostingPlan>();

    public virtual ICollection<User> InverseOwner { get; set; } = new List<User>();

    public virtual User Owner { get; set; }

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    public virtual ICollection<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
}
#endif