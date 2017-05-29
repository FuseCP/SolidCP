using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012
{
    public static class QuotaHelper
    {
        public static void CheckNumericQuota(PackageContext cntx, List<string> errors, string quotaName, long currentVal, long val, string messageKey)
        {
            CheckQuotaValue(cntx, errors, quotaName, currentVal, val, messageKey);
        }
        public static void CheckNumericQuota(PackageContext cntx, List<string> errors, string quotaName, int currentVal, int val, string messageKey)
        {
            CheckQuotaValue(cntx, errors, quotaName, Convert.ToInt64(currentVal), Convert.ToInt64(val), messageKey);
        }

        public static void CheckNumericQuota(PackageContext cntx, List<string> errors, string quotaName, int val, string messageKey)
        {
            CheckQuotaValue(cntx, errors, quotaName, 0, val, messageKey);
        }

        public static void CheckBooleanQuota(PackageContext cntx, List<string> errors, string quotaName, bool val, string messageKey)
        {
            CheckQuotaValue(cntx, errors, quotaName, 0, val ? 1 : 0, messageKey);
        }

        public static void CheckListsQuota(PackageContext cntx, List<string> errors, string quotaName, string messageKey)
        {
            CheckQuotaValue(cntx, errors, quotaName, 0, -1, messageKey);
        }

        public static void CheckQuotaValue(PackageContext cntx, List<string> errors, string quotaName, long currentVal, long val, string messageKey)
        {
            if (!cntx.Quotas.ContainsKey(quotaName))
                return;

            QuotaValueInfo quota = cntx.Quotas[quotaName];

            if (val == -1 && quota.QuotaExhausted) // check if quota already reached
            {
                errors.Add(messageKey + ":" + quota.QuotaAllocatedValue);
            }
            else if (quota.QuotaAllocatedValue == -1)
                return; // unlimited
            else if (quota.QuotaTypeId == 1 && val == 1 && quota.QuotaAllocatedValue == 0) // bool quota
                errors.Add(messageKey);
            else if (quota.QuotaTypeId == 2)
            {
                long maxValue = quota.QuotaAllocatedValue - quota.QuotaUsedValue + currentVal;
                if (val > maxValue)
                    errors.Add(messageKey + ":" + maxValue);
            }
            else if (quota.QuotaTypeId == 3 && val > quota.QuotaAllocatedValue)
            {
                int maxValue = quota.QuotaAllocatedValue;
                errors.Add(messageKey + ":" + maxValue);
            }
        }
    }
}
