#if NetFX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;


namespace SolidCP.EnterpriseServer.Data
{
	public static class NetFXConfigurationExtensions
	{
		/*
		public static PrimaryKeyIndexConfiguration HasKey<TEntity>(this EntityTypeConfiguration<TEntity> entity, Expression<Func<TEntity, object?>> keyExpression) where TEntity: class
		{
			PrimaryKeyIndexConfiguration conf = null;
			entity.HasKey(keyExpression, key => conf = key);
			return conf;
		}

		public static IndexConfiguration HasIndex<TEntity>(this EntityTypeConfiguration<TEntity> entity, Expression<Func<TEntity, object?>> indexExpression, string name) where TEntity: class => entity.HasIndex(indexExpression).HasName(name);

		public static object HasData<TEntity>(this EntityTypeConfiguration<TEntity> entity, params TEntity[] data) where TEntity: class => new object();
		*/

		public static PrimitivePropertyConfiguration ValueGeneratedOnAdd(this PrimitivePropertyConfiguration config) => config.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
	}
}
#endif