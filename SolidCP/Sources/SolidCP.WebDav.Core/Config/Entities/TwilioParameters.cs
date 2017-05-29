namespace SolidCP.WebDav.Core.Config.Entities
{
    public class TwilioParameters: AbstractConfigCollection
    {
        public string AccountSid { get; private set; }
        public string AuthorizationToken { get; private set; }
        public string PhoneFrom { get; private set; }

        public TwilioParameters()
        {
            AccountSid = ConfigSection.Twilio.AccountSid;
            AuthorizationToken = ConfigSection.Twilio.AuthorizationToken;
            PhoneFrom = ConfigSection.Twilio.PhoneFrom;
        }
    }
}