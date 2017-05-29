namespace SolidCP.WebDav.Core.Interfaces.Storages
{
    public interface IKeyValueStorage
    {
        TV Get<TV>(string id);
        bool Add<TV>(string id, TV value);
        bool Delete(string id);
    }
}