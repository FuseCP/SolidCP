using SolidCP.Providers.StorageSpaces;

namespace SolidCP.EnterpriseServer
{
    public interface IStorageSpaceSelector
    {
        StorageSpace FindBest(string groupName, long quotaSizeInBytes);
    }
}