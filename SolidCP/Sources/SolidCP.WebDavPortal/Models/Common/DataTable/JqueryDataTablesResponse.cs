using System.Collections;

namespace SolidCP.WebDavPortal.Models.Common.DataTable
{
    public class JqueryDataTablesResponse
    {
        public int draw { get; private set; }
        public IEnumerable data { get; private set; }
        public int recordsTotal { get; private set; }
        public int recordsFiltered { get; private set; }

        public JqueryDataTablesResponse(int draw, IEnumerable data, int recordsFilteredCount, int recordsTotalCount)
        {
            this.draw = draw;
            this.data = data;
            this.recordsFiltered = recordsFilteredCount;
            this.recordsTotal = recordsTotalCount;
        } 
    }
}