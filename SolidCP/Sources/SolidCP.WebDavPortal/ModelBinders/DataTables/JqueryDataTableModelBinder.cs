using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SolidCP.WebDavPortal.Models.Common.DataTable;


namespace SolidCP.WebDavPortal.ModelBinders.DataTables
{
    public class JqueryDataTableModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.BindModel(controllerContext, bindingContext);
            HttpRequestBase request = controllerContext.HttpContext.Request;

            // Retrieve request data
            int draw = Convert.ToInt32(request["draw"]);
            int start = Convert.ToInt32(request["start"]);
            int count = Convert.ToInt32(request["length"]);

            // Search
            var search = new JqueryDataTableSearch
            {
                Value = request["search[value]"],
                IsRegex = Convert.ToBoolean(request["search[regex]"])
            };

            var orderIndex = 0;

            var orders = new List<JqueryDataTableOrder>();

            while (request["order[" + orderIndex + "][column]"] != null)
            {
                orders.Add(new JqueryDataTableOrder()
                {
                    Column = Convert.ToInt32(request["order[" + orderIndex + "][column]"]),
                    Ascending = (request["order[" + orderIndex + "][dir]"] == "asc")
                });

                orderIndex++;
            }

            // Columns
            var columnsIndex = 0;
            var columns = new List<JqueryDataTableColumn>();

            while (request["columns[" + columnsIndex + "][name]"] != null)
            {
                columns.Add(new JqueryDataTableColumn
                {
                    Data = request["columns[" + columnsIndex + "][data]"],
                    Name = request["columns[" + columnsIndex + "][name]"],
                    Orderable = Convert.ToBoolean(request["columns[" + columnsIndex + "][orderable]"]),
                    Search = new JqueryDataTableSearch
                    {
                        Value = request["columns[" + columnsIndex + "][search][value]"],
                        IsRegex = Convert.ToBoolean(request["columns[" + columnsIndex + "][search][regex]"])
                    }
                });

                columnsIndex++;
            }

            return new JqueryDataTableRequest
            {
                Draw = draw,
                Start = start,
                Count = count,
                Search = search,
                Orders = orders,
                Columns = columns
            };
        }
         
    }
}