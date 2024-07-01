using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#if NETFRAMEWORK
using System.Data.Entity;
#endif
#if !NETFRAMEWORK && !NETSTANDARD
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
#endif


namespace SolidCP.EnterpriseServer.Data
{
	public interface IGenericDbContext : IDisposable
	{
#if NETFRAMEWORK
		System.Data.Entity.DbSet<TEntity> Set<TEntity>() where TEntity : class;
#endif
#if !NETFRAMEWORK && !NETSTANDARD
        Microsoft.EntityFrameworkCore.DbSet<TEntity> Set<TEntity>() where TEntity : class;
#endif
		int SaveChanges();
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
		Action<string> Log { get; set; }

#if NETFRAMEWORK
		Database Database { get; }
#else
        DatabaseFacade Database { get; }
#endif
	}
}
