#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class UsersDetailed
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public int StatusId { get; set; }

    public int? LoginStatusId { get; set; }

    public string SubscriberNumber { get; set; }

    public int? FailedLogins { get; set; }

    public int? OwnerId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Changed { get; set; }

    public bool IsDemo { get; set; }

    public string Comments { get; set; }

    public bool IsPeer { get; set; }

    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string CompanyName { get; set; }

    public string FullName { get; set; }

    public string OwnerUsername { get; set; }

    public string OwnerFirstName { get; set; }

    public string OwnerLastName { get; set; }

    public int? OwnerRoleId { get; set; }

    public string OwnerFullName { get; set; }

    public string OwnerEmail { get; set; }

    public int? Expr1 { get; set; }

    public int? PackagesNumber { get; set; }

    public bool? EcommerceEnabled { get; set; }
}
#endif