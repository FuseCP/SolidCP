namespace SolidCP.WebDav.Core.Interfaces.Services
{
    public interface ISmsDistributionService
    {
        bool SendMessage(string phoneFrom, string phone, string message);

        bool SendMessage(string phone, string message); 
    }
}