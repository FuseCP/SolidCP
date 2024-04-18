using System;
using System.Collections.Generic;
#if !NETFRAMEWORK && !NETSTANDARD
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#endif
#if NETFRAMEWORK && !NETSTANDARD
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class PackageResourceConfiguration
{
}
