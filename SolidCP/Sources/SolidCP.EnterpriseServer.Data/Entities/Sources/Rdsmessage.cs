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

[Table("RDSMessages")]
#if NetCore
[Index("RdscollectionId", Name = "RDSMessagesIdx_RDSCollectionId")]
#endif
public partial class Rdsmessage
{
    [Key]
    public int Id { get; set; }

    [Column("RDSCollectionId")]
    public int RdscollectionId { get; set; }

    [Required]
    [Column(TypeName = "ntext")]
    public string MessageText { get; set; }

    [Required]
    [StringLength(250)]
    public string UserName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [ForeignKey("RdscollectionId")]
    [InverseProperty("Rdsmessages")]
    public virtual Rdscollection Rdscollection { get; set; }
}
#endif