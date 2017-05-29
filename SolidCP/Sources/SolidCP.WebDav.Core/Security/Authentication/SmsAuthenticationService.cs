using System;
using System.Globalization;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Interfaces.Security;
using SolidCP.WebDav.Core.Interfaces.Services;

namespace SolidCP.WebDav.Core.Security.Authentication
{
    public class SmsAuthenticationService : ISmsAuthenticationService
    {
        private ISmsDistributionService _smsService;

        public SmsAuthenticationService(ISmsDistributionService smsService)
        {
            _smsService = smsService;
        }

        public bool VerifyResponse( Guid token, string response)
        {
            var accessToken = ScpContext.Services.Organizations.GetPasswordresetAccessToken(token);

            if (accessToken == null)
            {
                return false;
            }

            return string.Compare(accessToken.SmsResponse, response, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public string SendRequestMessage(string phoneTo)
        {
            var response = GenerateResponse();

            var result = _smsService.SendMessage(WebDavAppConfigManager.Instance.TwilioParameters.PhoneFrom, phoneTo, response);

            return result ? response : string.Empty;
        }

        public string GenerateResponse()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());

            return random.Next(10000, 99999).ToString(CultureInfo.InvariantCulture);
        }
    }
}