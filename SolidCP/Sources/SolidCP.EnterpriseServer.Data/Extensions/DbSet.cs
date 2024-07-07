#if CustomDbSet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections;
using System.Threading;
using SolidCP.Providers.OS;
#if NETFRAMEWORK
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data
{
    public class DbSet<TEntity> : IQueryable<TEntity>, IEnumerable<TEntity>, IEnumerable, IQueryable where TEntity : class
    {
        const int EntityStateModified = 0x10;

        public IGenericDbContext BaseContext { get; set; }

        IQueryable<TEntity> set = null;
#if !NETSTANDARD
        IQueryable<TEntity> Set => set ?? (set = BaseContext.Set<TEntity>());
#else
        IQueryable<TEntity> Set {
            get {
                if (set == null) { // use reflection to call Set<TEntity>()
                    set = Invoke<IQueryable<TEntity>>(BaseContext, nameof(Set), new Type[0]);
                }
                return set;
            }
        }
#endif

                    #region Helper methods
                    MethodInfo GetMethod(object obj, string method, Type[] types) => obj.GetType().GetMethod(method, types);
        T Invoke<T>(object obj, MethodInfo method, params object[] args) => (T)method.Invoke(obj, args);
        T Invoke<T>(object obj, string method, Type[] types, params object[] args) => (T)Invoke<T>(obj, GetMethod(obj, method, types), args);
        T Invoke<T, U>(object obj, string method, Type[] types, params object[] args) => (T)Invoke<T>(obj, GetMethod(obj, method, types).MakeGenericMethod(typeof(U)), args);
        #endregion

        public DbSet(IGenericDbContext context) {
            BaseContext = context;
            set = Set;
        }

        static Type[] TypesOfTEntity = new Type[] { typeof(TEntity) };
        public TEntity Add(TEntity entity) => Invoke<TEntity>(Set, nameof(Add), TypesOfTEntity, entity);
        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities) => Invoke<IEnumerable<TEntity>>(Set, nameof(AddRange), new Type[] { typeof(IEnumerable<TEntity>) }, entities);
        public void AddRange(params TEntity[] entities) => AddRange((IEnumerable<TEntity>)entities);
        public void Attach(TEntity entity) => Invoke<object>(Set, nameof(Attach), TypesOfTEntity, entity);
        public void AttachRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities) Attach(entity);
        }
        public void AttachRange(params TEntity[] entities) => AttachRange((IEnumerable<TEntity>)entities);
        public TEntity? Find(params object?[]? keyValues) => Invoke<TEntity>(Set, nameof(Find), new Type[] { typeof(object[]) }, keyValues);
        public async ValueTask<TEntity?> FindAsync(object?[]? keyValues, CancellationToken cancellationToken)
        {
            var task = Invoke<object>(Set, nameof(FindAsync), new Type[] { typeof(object[]), typeof(CancellationToken) }, keyValues, cancellationToken);
            if (task is Task<TEntity?> t) return await t;
            else if (task is ValueTask<TEntity?> vt) return await vt;
            else throw new InvalidOperationException("Invalid return type of FindAsync method");
        }
        public ValueTask<TEntity?> FindAsync(params object?[]? keyValues) => FindAsync(keyValues, CancellationToken.None);
        public ICollection<TEntity> Local => (ICollection<TEntity>)Set.GetType().GetProperty(nameof(Local))?.GetValue(Set);
        public void Remove(TEntity entity) => Invoke<object>(Set, nameof(Remove), TypesOfTEntity, entity);
        public void RemoveRange(IEnumerable<TEntity> entities) => Invoke<object>(Set, nameof(Remove), new Type[] { typeof(IEnumerable<TEntity>) }, entities);
        public void RemoveRange(params TEntity[] entities) => RemoveRange((IEnumerable<TEntity>)entities);
        public void Update(TEntity entity)
        {
            if (OSInfo.IsCore) Invoke<object>(Set, nameof(Update), TypesOfTEntity, entity);
            else
            {
                Attach(entity);
                var entry = Invoke<object>(BaseContext, "Entry", TypesOfTEntity, entity);
                entry.GetType().GetProperty("State").SetValue(entry, EntityStateModified);
            }
        }
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (OSInfo.IsCore) Invoke<object>(Set, nameof(Update), TypesOfTEntity, entities);
            else
            {
                foreach (var entity in entities)
                {
                    Update(entity);
                }
            }
        }
        public void UpdateRange(params TEntity[] entities) => UpdateRange((IEnumerable<TEntity>)entities);

        #region IQueryable methods
        public Expression Expression => Set.Expression;
        public Type ElementType => Set.ElementType;
        public IQueryProvider Provider => Set.Provider;
        Expression IQueryable.Expression => ((IQueryable)Set).Expression;
        public IEnumerator<TEntity> GetEnumerator() => Set.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)Set).GetEnumerator();
        }
        #endregion
    }
}
#endif