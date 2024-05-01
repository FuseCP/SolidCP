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
//[Keyless]
#endif
public partial class Theme
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ThemeID")]
    public int ThemeId { get; set; }

    [StringLength(255)]
    public string DisplayName { get; set; }

    [Column("LTRName")]
    [StringLength(255)]
    public string LTRName { get; set; }

    [Column("RTLName")]
    [StringLength(255)]
    public string RTLName { get; set; }

    public int Enabled { get; set; }

    public int DisplayOrder { get; set; }
}
#endif