#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("RDSMessages")]
#if NetCore
[Index("RdsCollectionId", Name = "RDSMessagesIdx_RDSCollectionId")]
#endif
public partial class RdsMessage
{
    [Key]
    public int Id { get; set; }

    [Column("RDSCollectionId")]
    public int RdsCollectionId { get; set; }

    [Required(AllowEmptyStrings = true)]
    //[Column(TypeName = "ntext")]
    public string MessageText { get; set; }

    [Required]
    [StringLength(250)]
    public string UserName { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [ForeignKey("RdsCollectionId")]
    [InverseProperty("RdsMessages")]
    public virtual RdsCollection RdsCollection { get; set; }
}
#endif