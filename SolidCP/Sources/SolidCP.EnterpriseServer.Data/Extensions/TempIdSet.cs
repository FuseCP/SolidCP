using SolidCP.EnterpriseServer.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

#if NETCOREAPP
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data
{
	public class TempIdSet: IQueryable<int>, IDisposable
	{
		public Guid Scope { get; private set; }
		public IQueryable<int> Query { get; private set; }

		public Expression Expression => Query.Expression;

		public Type ElementType => Query.ElementType;

		public IQueryProvider Provider => Query.Provider;

		public DbContext Context { get; private set; }

		public TempIdSet(DbContext context, Guid scope = default(Guid), int level = 0)
		{

			Scope = scope == default ? Guid.NewGuid() : scope;
			Context = context;

			if (level == 0)
			{
				Query = context.TempIds
					.Where(id => id.Scope == Scope)
					.Select(id => id.Id);
			}
			else
			{
				Query = context.TempIds
					.Where(id => id.Scope == Scope && id.Level == level)
					.Select(id => id.Id);
			}
		}

		public TempIdSet(DbContext context, IEnumerable<int> ids, Guid scope = default(Guid), int level = 0):
			this(context, scope, level) => AddRange(ids, level);

		public IQueryable<int> OfLevel(int level)
		{
			if (level == 0)
			{
				return Context.TempIds
					.Where(id => id.Scope == Scope)
					.Select(id => id.Id);
			}
			else
			{
				return Context.TempIds
					.Where(id => id.Scope == Scope && id.Level == level)
					.Select(id => id.Id);
			}
		}

		public IQueryable<TempId> TempIds(int level = 0)
		{
			if (level == 0)
			{
				return Context.TempIds
					.Where(id => id.Scope == Scope);
			}
			else
			{
				return Context.TempIds
					.Where(id => id.Scope == Scope && id.Level == level);
			}
		}
		public virtual void Add(int id, int level = 0)
		{
			var tempId = new TempId()
			{
				Id = id,
				Scope = Scope,
				Level = level,
				Created = DateTime.Now,
				Date = default
			};
			Context.TempIds.Add(tempId);
		}

		public virtual int AddRange(IEnumerable<int> ids, int level = 0)
		{
			int n = 0;
			var queryable = ids is IQueryable<int>;
			if (!queryable || !AddRangeQueryable((IQueryable<int>)ids, out n, level))
			{
				const int BatchSize = 1024;
				var buffer = new TempId[BatchSize];
				var created = DateTime.Now;
				var tempIds = ids
					.Select(id => new TempId()
					{
						Id = id,
						Scope = Scope,
						Level = level,
						Created = created,
						Date = default
					});
				var enumerator = tempIds.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int i = 0;
					do
					{
						buffer[i++] = enumerator.Current;
					} while (i < BatchSize && enumerator.MoveNext());
					using (var context = new DbContext())
					{
						context.TempIds.AddRange(i == BatchSize ? buffer : buffer.Take(i));
						n += context.SaveChanges();
					}
				}
			}
			return n;
		}

		protected virtual bool AddRangeQueryable(IQueryable<int> ids, out int n, int level = 0)
		{
			n = 0;
			return false;
			var created = DateTime.Now;
			var tempIds = ids
				.Select(id => new TempId()
				{
					Id = id,
					Scope = Scope,
					Level = level,
					Created = created,
					Date = default
				});
		}

		public void Dispose()
		{
			Task.Run(async () =>
			{
				await Task.Delay(TimeSpan.FromSeconds(2));
				var now = DateTime.Now;
				var old = now.Subtract(TimeSpan.FromSeconds(30));
				Context.TempIds
					.Where(id => id.Scope == Scope || id.Created < old)
					.ExecuteDelete();
			});
		}

		public IEnumerator<int> GetEnumerator() => Query.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Query).GetEnumerator();
	}

	public class DatedId
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
	}

	public class TempDatedIdSet : IQueryable<DatedId>, IDisposable
	{
		public Guid Scope { get; private set; }
		public IQueryable<DatedId> Query { get; private set; }

		public Expression Expression => Query.Expression;

		public Type ElementType => Query.ElementType;

		public IQueryProvider Provider => Query.Provider;

		public DbContext Context { get; private set; }

		public TempDatedIdSet(DbContext context, Guid scope = default(Guid), int level = 0)
		{

			Scope = scope == default ? Guid.NewGuid() : scope;
			Context = context;

			if (level == 0)
			{
				Query = context.TempIds
					.Where(id => id.Scope == Scope)
					.Select(id => new DatedId { Id = id.Id, Date = id.Date });
			}
			else
			{
				Query = context.TempIds
					.Where(id => id.Scope == Scope && id.Level == level)
					.Select(id => new DatedId { Id = id.Id, Date = id.Date });
			}
		}

		public TempDatedIdSet(DbContext context, IEnumerable<DatedId> ids, Guid scope = default(Guid), int level = 0) :
			this(context, scope, level) => AddRange(ids, level);

		public IQueryable<DatedId> OfLevel(int level)
		{
			if (level == 0)
			{
				return Context.TempIds
					.Where(id => id.Scope == Scope)
					.Select(id => new DatedId { Id = id.Id, Date = id.Date });
			}
			else
			{
				return Context.TempIds
					.Where(id => id.Scope == Scope && id.Level == level)
					.Select(id => new DatedId { Id = id.Id, Date = id.Date });
			}
		}

		public IQueryable<TempId> TempIds(int level = 0)
		{
			if (level == 0)
			{
				return Context.TempIds
					.Where(id => id.Scope == Scope);
			}
			else
			{
				return Context.TempIds
					.Where(id => id.Scope == Scope && id.Level == level);
			}
		}
		public virtual void Add(DatedId id, int level = 0)
		{
			var tempId = new TempId()
			{
				Id = id.Id,
				Scope = Scope,
				Level = level,
				Created = DateTime.Now,
				Date = id.Date
			};
			Context.TempIds.Add(tempId);
		}

		public virtual int AddRange(IEnumerable<DatedId> ids, int level = 0)
		{
			int n = 0;
			var queryable = ids is IQueryable<int>;
			if (!queryable || !AddRangeQueryable((IQueryable<DatedId>)ids, out n, level))
			{
				const int BatchSize = 1024;
				var buffer = new TempId[BatchSize];
				var created = DateTime.Now;
				var tempIds = ids
					.Select(id => new TempId()
					{
						Id = id.Id,
						Scope = Scope,
						Level = level,
						Created = created,
						Date = id.Date
					});
				var enumerator = tempIds.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int i = 0;
					do
					{
						buffer[i++] = enumerator.Current;
					} while (i < BatchSize && enumerator.MoveNext());
					using (var context = new DbContext())
					{
						context.TempIds.AddRange(i == BatchSize ? buffer : buffer.Take(i));
						n += context.SaveChanges();
					}
				}
			}
			return n;
		}

		// can use an optimized bulk instert
		protected virtual bool AddRangeQueryable(IQueryable<DatedId> ids, out int n, int level = 0)
		{
			n = 0;
			return false;
			var created = DateTime.Now;
			var tempIds = ids
				.Select(id => new TempId()
				{
					Id = id.Id,
					Scope = Scope,
					Level = level,
					Created = created,
					Date = id.Date
				});
		}

		public void Dispose()
		{
			Task.Run(async () =>
			{
				await Task.Delay(TimeSpan.FromSeconds(2));
				var now = DateTime.Now;
				var old = now.Subtract(TimeSpan.FromSeconds(30));
				Context.TempIds
					.Where(id => id.Scope == Scope || id.Created < old)
					.ExecuteDelete();
			});
		}

		public IEnumerator<DatedId> GetEnumerator() => Query.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Query).GetEnumerator();
	}

	public static class EnumerableExtensions
	{
		public static TempIdSet ToTempIdSet(this IEnumerable<int> ids, DbContext context, Guid scope = default(Guid), int level = 0)
		{
			return new TempIdSet(context, ids, scope, level);
		}
		public static TempDatedIdSet ToTempDatedIdSet(this IEnumerable<DatedId> ids, DbContext context, Guid scope = default(Guid), int level = 0)
		{
			return new TempDatedIdSet(context, ids, scope, level);
		}
	}
}
