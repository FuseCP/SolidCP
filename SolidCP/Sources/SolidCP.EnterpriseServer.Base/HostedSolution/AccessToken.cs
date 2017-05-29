using System;

namespace SolidCP.EnterpriseServer.Base.HostedSolution
{
    public class AccessToken
    {
        public int Id { get; set; }
        public Guid AccessTokenGuid { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int AccountId { get; set; }
        public int ItemId { get; set; }
        public AccessTokenTypes TokenType { get; set; }
        public string SmsResponse { get; set; }
        public bool IsSmsSent {
            get { return !string.IsNullOrEmpty(SmsResponse); }
        }
    }
}