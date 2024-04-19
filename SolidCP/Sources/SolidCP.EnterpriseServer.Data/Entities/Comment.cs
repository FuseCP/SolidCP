// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Comment
{
    public int CommentId { get; set; }

    public string ItemTypeId { get; set; }

    public int ItemId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CommentText { get; set; }

    public int? SeverityId { get; set; }

    public virtual User User { get; set; }
}
#endif