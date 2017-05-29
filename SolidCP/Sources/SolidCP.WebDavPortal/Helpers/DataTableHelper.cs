using System;
using System.Collections.Generic;
using System.Linq;
using SolidCP.WebDavPortal.Models.Common.DataTable;

namespace SolidCP.WebDavPortal.Helpers
{
    public class DataTableHelper
    {
        public static JqueryDataTablesResponse ProcessRequest<TEntity>(IEnumerable<TEntity> entities, JqueryDataTableRequest request) where TEntity : JqueryDataTableBaseEntity
        {
            IOrderedEnumerable<TEntity> orderedEntities = null;

            foreach (var order in request.Orders)
            {
                var closure = order;

                if (orderedEntities == null)
                {
                    orderedEntities = order.Ascending ? entities.OrderBy(x => x[closure.Column]) : entities.OrderByDescending(x => x[closure.Column]);
                }
                else
                {
                    orderedEntities = order.Ascending ? orderedEntities.ThenBy(x => x[closure.Column]) : orderedEntities.ThenByDescending(x => x[closure.Column]);
                }
            }

            if (orderedEntities == null)
            {
                orderedEntities = entities.OrderBy(x=>x[0]);
            }

            var itemsPaged = orderedEntities.Skip(request.Start).Take(request.Count).ToList();
            var totalCount = orderedEntities.Count();


            return new JqueryDataTablesResponse(
                request.Draw,
                itemsPaged,
                totalCount,
                totalCount);
        } 
    }
}