using System.Collections;
using System.Collections.Generic;

namespace SolidCP.WebDavPortal.Models.Common.DataTable
{
    public abstract class JqueryDataTableBaseEntity 
    {
        public abstract dynamic this[int index]
        {
            get; 
        }
    }
}