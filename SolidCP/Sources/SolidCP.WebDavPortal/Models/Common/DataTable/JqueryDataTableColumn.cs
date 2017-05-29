namespace SolidCP.WebDavPortal.Models.Common.DataTable
{
    public class JqueryDataTableColumn
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public bool Orderable { get; set; }

        public JqueryDataTableSearch Search { get; set; } 
    }
}