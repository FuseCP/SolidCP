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
[Index("TaskId", Name = "BackgroundTaskParametersIdx_TaskID")]
#endif
public partial class BackgroundTaskParameter
{
    [Key]
    [Column("ParameterID")]
    public int ParameterId { get; set; }

    [Column("TaskID")]
    public int TaskId { get; set; }

    [StringLength(255)]
    public string Name { get; set; }

    [Column(TypeName = "ntext")]
    public string SerializerValue { get; set; }

    [StringLength(255)]
    public string TypeName { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("BackgroundTaskParameters")]
    public virtual BackgroundTask Task { get; set; }
}
#endif