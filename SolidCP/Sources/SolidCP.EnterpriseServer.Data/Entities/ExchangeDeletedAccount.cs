#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ExchangeDeletedAccount
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public int OriginAt { get; set; }

    public string StoragePath { get; set; }

    public string FolderName { get; set; }

    public string FileName { get; set; }

    public DateTime ExpirationDate { get; set; }
}
#endif