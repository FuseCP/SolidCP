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
[PrimaryKey("ScheduleId", "ParameterId")]
#endif
public partial class ScheduleParameter
{
    [Key]
    [Column("ScheduleID", Order = 1)]
    public int ScheduleId { get; set; }

    [Key]
    [Column("ParameterID", Order = 2)]
    [StringLength(100)]
    public string ParameterId { get; set; }

    [StringLength(1000)]
    public string ParameterValue { get; set; }

    [ForeignKey("ScheduleId")]
    [InverseProperty("ScheduleParameters")]
    public virtual Schedule Schedule { get; set; }
}
#endif