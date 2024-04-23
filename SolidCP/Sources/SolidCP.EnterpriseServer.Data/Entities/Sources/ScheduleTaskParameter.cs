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
[PrimaryKey("TaskId", "ParameterId")]
#endif
public partial class ScheduleTaskParameter
{
    [Key]
    [Column("TaskID")]
    [StringLength(100)]
    public string TaskId { get; set; }

    [Key]
    [Column("ParameterID")]
    [StringLength(100)]
    public string ParameterId { get; set; }

    [Required]
    [Column("DataTypeID")]
    [StringLength(50)]
    public string DataTypeId { get; set; }

    public string DefaultValue { get; set; }

    public int ParameterOrder { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("ScheduleTaskParameters")]
    public virtual ScheduleTask Task { get; set; }
}
#endif