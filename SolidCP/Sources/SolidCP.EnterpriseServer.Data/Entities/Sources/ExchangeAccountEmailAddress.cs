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
[Index("AccountId", Name = "ExchangeAccountEmailAddressesIdx_AccountID")]
[Index("EmailAddress", Name = "IX_ExchangeAccountEmailAddresses_UniqueEmail", IsUnique = true)]
#endif
public partial class ExchangeAccountEmailAddress
{
    [Key]
    [Column("AddressID")]
    public int AddressId { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    [Required]
    [StringLength(300)]
    public string EmailAddress { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("ExchangeAccountEmailAddresses")]
    public virtual ExchangeAccount Account { get; set; }
}
#endif