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
[Index("UserId", Name = "CommentsIdx_UserID")]
#endif
public partial class Comment
{
    [Key]
    [Column("CommentID")]
    public int CommentId { get; set; }

    [Required]
    [Column("ItemTypeID")]
    [StringLength(50)]
#if NetCore
    [Unicode(false)]
#endif
    public string ItemTypeId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [StringLength(1000)]
    public string CommentText { get; set; }

    [Column("SeverityID")]
    public int? SeverityId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("CommentsNavigation")]
    public virtual User User { get; set; }
}
#endif