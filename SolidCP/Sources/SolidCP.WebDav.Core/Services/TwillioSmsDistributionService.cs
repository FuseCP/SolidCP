using Twilio;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Interfaces.Services;

namespace SolidCP.WebDav.Core.Services
{
    public class TwillioSmsDistributionService : ISmsDistributionService
    {
        private readonly TwilioRestClient _twilioRestClient;

        public TwillioSmsDistributionService()
        {
            _twilioRestClient = new TwilioRestClient(WebDavAppConfigManager.Instance.TwilioParameters.AccountSid, WebDavAppConfigManager.Instance.TwilioParameters.AuthorizationToken);
        }


        public bool SendMessage(string phoneFrom, string phone, string message)
        {
            var result = _twilioRestClient.SendSmsMessage(phoneFrom, phone, message);

            return string.IsNullOrEmpty(result.Status) == false;
        }

        public bool SendMessage(string phone, string message)
        {
            var result = _twilioRestClient.SendSmsMessage(WebDavAppConfigManager.Instance.TwilioParameters.PhoneFrom, phone, message);

            return string.IsNullOrEmpty(result.Status) == false;
        }
    }
}