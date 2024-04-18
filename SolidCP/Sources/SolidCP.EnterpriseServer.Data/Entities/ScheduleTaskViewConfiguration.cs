#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[PrimaryKey("ConfigurationId", "TaskId")]
[Table("ScheduleTaskViewConfiguration")]
public partial class ScheduleTaskViewConfiguration
{
    [Key]
    [Column("TaskID")]
    [StringLength(100)]
    public string TaskId { get; set; }

    [Key]
    [Column("ConfigurationID")]
    [StringLength(100)]
    public string ConfigurationId { get; set; }

    [Required]
    [StringLength(100)]
    public string Environment { get; set; }

    [Required]
    [StringLength(100)]
    public string Description { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("ScheduleTaskViewConfigurations")]
    public virtual ScheduleTask Task { get; set; }
}
#endif