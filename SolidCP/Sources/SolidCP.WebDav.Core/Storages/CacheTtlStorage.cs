using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Microsoft.Web.Services3.Design;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Interfaces.Storages;

namespace SolidCP.WebDav.Core.Storages
{
    public class CacheTtlStorage : ITtlStorage
    {
       private static readonly ObjectCache Cache;

       static CacheTtlStorage()
        {
            Cache = MemoryCache.Default;
        }

        public TV Get<TV>(string id)
        {
            var value = (TV)Cache[id];

            if (!EqualityComparer<TV>.Default.Equals(value, default(TV)))
            {
                SetTtl(id, value);
            }

            return value;
        }

        public bool Add<TV>(string id, TV value)
        {
            return Cache.Add(id, value, DateTime.Now.AddMinutes(WebDavAppConfigManager.Instance.OfficeOnline.CobaltFileTtl));
        }

        public bool Delete(string id)
        {
            if (Cache.Any(x => x.Key == id))
            {
                Cache.Remove(id);

                return true;
            }

            return false;
        }

        public void SetTtl<TV>(string id, TV value)
        {
            Cache.Set(id, value, DateTime.Now.AddMinutes(WebDavAppConfigManager.Instance.OfficeOnline.CobaltFileTtl));
        }
    }
}