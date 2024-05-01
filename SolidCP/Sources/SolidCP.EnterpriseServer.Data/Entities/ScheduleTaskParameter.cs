#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

#if NetCore
[PrimaryKey("TaskId", "ParameterId")]
#endif
public partial class ScheduleTaskParameter
{
    [Key]
    [Column("TaskID", Order = 1)]
    [StringLength(100)]
    public string TaskId { get; set; }

    [Key]
    [Column("ParameterID", Order = 2)]
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