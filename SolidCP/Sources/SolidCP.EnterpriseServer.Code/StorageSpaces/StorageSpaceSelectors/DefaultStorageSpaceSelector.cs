using System;
using System.Linq;
using SolidCP.Providers.StorageSpaces;

namespace SolidCP.EnterpriseServer
{
    public class DefaultStorageSpaceSelector : IStorageSpaceSelector
    {
        public StorageSpace FindBest(string groupName, long quotaSizeInBytes)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException("groupName");
            }

            var storages = ObjectUtils.CreateListFromDataReader<StorageSpace>(DataProvider.GetStorageSpacesByResourceGroupName(groupName)).Where(x=> !x.IsDisabled).ToList();

            if (!storages.Any())
            {
                throw new Exception(string.Format("Storage spaces not found for '{0}' resource group", groupName));
            }

            var orderedStorages = storages.OrderByDescending(x => x.FsrmQuotaSizeBytes - x.UsedSizeBytes);

            var bestStorage = orderedStorages.First();

            if (bestStorage.FsrmQuotaSizeBytes - bestStorage.UsedSizeBytes < quotaSizeInBytes)
            {
                throw new Exception("Space storages was found, but available space not enough");
            }

            return bestStorage;
        }
    }
}