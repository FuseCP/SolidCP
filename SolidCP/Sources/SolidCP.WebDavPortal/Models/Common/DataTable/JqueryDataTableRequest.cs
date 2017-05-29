using System.Collections.Generic;

namespace SolidCP.WebDavPortal.Models.Common.DataTable
{
    public class JqueryDataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Count { get; set; }


        public JqueryDataTableSearch Search { get; set; }
        public IEnumerable<JqueryDataTableOrder> Orders { get; set; }
        public IEnumerable<JqueryDataTableColumn> Columns { get; set; } 
    }
}