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
[Index("Created", "Scope", "Level", Name = "IX_TempIds_Created_Scope_Level")]
#endif
public partial class TempId
{
    [Key]
    public int Key { get; set; }

    public DateTime Created { get; set; }

    public Guid Scope { get; set; }

    public int Level { get; set; }

    public int Id { get; set; }

    public DateTime Date { get; set; }
}
#endif