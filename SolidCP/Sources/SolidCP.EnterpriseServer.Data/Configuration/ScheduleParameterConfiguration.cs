using System;
using System.Collections.Generic;
#if NetCore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#endif
#if NetFX
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

#if NetCore
public partial class ScheduleParameterConfiguration //: IEntityTypeConfiguration<ScheduleParameter>
#else
public partial class ScheduleParameterConfiguration //:
#endif
{

#if NetCore

    //public partial void 

#endif

}
