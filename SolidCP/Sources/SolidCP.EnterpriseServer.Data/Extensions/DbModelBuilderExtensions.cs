using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NetFX
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration
{
	public static class DbModelBuilderExtensions
	{
#if NetFX
		public static void ApplyConfiguration<T>(this DbModelBuilder builder, EntityTypeConfiguration<T> configuration) where T : class
		{
			builder.Configurations.Add(configuration);
		}
#endif
	}
}
