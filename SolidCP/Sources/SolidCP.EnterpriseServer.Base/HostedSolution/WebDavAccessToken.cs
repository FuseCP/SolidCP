using System;

namespace SolidCP.EnterpriseServer.Base.HostedSolution
{
    public class WebDavAccessToken
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string AuthData { get; set; }
        public Guid AccessToken { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int AccountId { get; set; }
        public int ItemId { get; set; }
    }
}