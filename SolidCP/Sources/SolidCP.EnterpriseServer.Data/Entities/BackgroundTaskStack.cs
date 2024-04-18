#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("BackgroundTaskStack")]
[Index("TaskId", Name = "BackgroundTaskStackIdx_TaskID")]
public partial class BackgroundTaskStack
{
    [Key]
    [Column("TaskStackID")]
    public int TaskStackId { get; set; }

    [Column("TaskID")]
    public int TaskId { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("BackgroundTaskStacks")]
    public virtual BackgroundTask Task { get; set; }
}
#endif