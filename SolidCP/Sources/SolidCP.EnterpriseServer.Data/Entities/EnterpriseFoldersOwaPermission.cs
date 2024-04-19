// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class EnterpriseFoldersOwaPermission
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public int FolderId { get; set; }

    public int AccountId { get; set; }

    public virtual ExchangeAccount Account { get; set; }

    public virtual EnterpriseFolder Folder { get; set; }
}
#endif