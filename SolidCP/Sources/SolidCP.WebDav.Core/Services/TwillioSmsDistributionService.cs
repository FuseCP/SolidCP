using Twilio;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Interfaces.Services;
using Twilio.Rest.Api.V2010.Account;

namespace SolidCP.WebDav.Core.Services
{
    public class TwillioSmsDistributionService : ISmsDistributionService
    {
        //private readonly TwilioRestClient _twilioRestClient;

        public TwillioSmsDistributionService()
        {
            //_twilioRestClient = new TwilioRestClient(WebDavAppConfigManager.Instance.TwilioParameters.AccountSid, WebDavAppConfigManager.Instance.TwilioParameters.AuthorizationToken);
               
        }


        public bool SendMessage(string phoneFrom, string phone, string message)
        {
            string accountSid = WebDavAppConfigManager.Instance.TwilioParameters.AccountSid;
            string authToken = WebDavAppConfigManager.Instance.TwilioParameters.AuthorizationToken;

            TwilioClient.Init(accountSid, authToken);

            var result = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(phoneFrom),
                to: new Twilio.Types.PhoneNumber(phone)
            );

            return string.IsNullOrEmpty(result.Status.ToString()) == false;


        }

        public bool SendMessage(string phone, string message)
        {
            string accountSid = WebDavAppConfigManager.Instance.TwilioParameters.AccountSid;
            string authToken = WebDavAppConfigManager.Instance.TwilioParameters.AuthorizationToken;

            TwilioClient.Init(accountSid, authToken);

            var result = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(WebDavAppConfigManager.Instance.TwilioParameters.PhoneFrom),
                to: new Twilio.Types.PhoneNumber(phone)
            );

            return string.IsNullOrEmpty(result.Status.ToString()) == false;
        }
    }
}