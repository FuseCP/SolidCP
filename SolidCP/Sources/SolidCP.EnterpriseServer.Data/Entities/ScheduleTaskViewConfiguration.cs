#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("ScheduleTaskViewConfiguration")]
#if NetCore
[PrimaryKey("ConfigurationId", "TaskId")]
#endif
public partial class ScheduleTaskViewConfiguration
{
    [Key]
    [Column("TaskID", Order = 1)]
    [StringLength(100)]
    public string TaskId { get; set; }

    [Key]
    [Column("ConfigurationID", Order = 2)]
    [StringLength(100)]
    public string ConfigurationId { get; set; }

    [Required]
    [StringLength(100)]
    public string Environment { get; set; }

    [Required(AllowEmptyStrings = true)]
    [StringLength(100)]
    public string Description { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("ScheduleTaskViewConfigurations")]
    public virtual ScheduleTask Task { get; set; }
}
#endif