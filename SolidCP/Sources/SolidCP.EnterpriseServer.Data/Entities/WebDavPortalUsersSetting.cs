#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Index("AccountId", Name = "WebDavPortalUsersSettingsIdx_AccountId")]
public partial class WebDavPortalUsersSetting
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string Settings { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("WebDavPortalUsersSettings")]
    public virtual ExchangeAccount Account { get; set; }
}
#endif