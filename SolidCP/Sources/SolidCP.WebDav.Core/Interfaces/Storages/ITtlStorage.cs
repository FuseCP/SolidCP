namespace SolidCP.WebDav.Core.Interfaces.Storages
{
    public interface ITtlStorage : IKeyValueStorage
    {
        void SetTtl<TV>(string id, TV value);
    }
}