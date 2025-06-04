// File: Services/TwilioService.cs

using Microsoft.Extensions.Configuration;
using TailorAPI.Repositories;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TailorAPI.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhone;
        private readonly string _whatsappNumber;
        //private readonly string _delayedMessageSid;
        //private readonly string _preCompletionMessageSid;
        //private readonly string _messageSid;
        
     

        public TwilioService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"]
                          ?? throw new ArgumentNullException("Twilio:AccountSid");

            _authToken = configuration["Twilio:AuthToken"]
                          ?? throw new ArgumentNullException("Twilio:AuthToken");

            _fromPhone = configuration["Twilio:FromPhoneNumber"]
                          ?? throw new ArgumentNullException("Twilio:FromPhoneNumber");

            _whatsappNumber = configuration["Twilio:WhatsAppNumber"]
                          ?? throw new ArgumentException("Twilio:WhatsAppNumber");

            //_delayedMessageSid = configuration["Twilio:DelayedMessageSid"]
            //              ?? throw new ArgumentException("Twilio:DelayedMessageSid");

            //_preCompletionMessageSid = configuration["Twilio:PreCompleteMessageSid"]
            //              ?? throw new ArgumentException("Twilio:PreCompleteMessageSid");

            //_messageSid = configuration["Twilio:MessageSid"]
            //              ?? throw new ArgumentException("Twilio:MessageSid");
            

            


            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task<string> SendSmsAsync(string toPhoneNumber, string message)
        {
            var result = await MessageResource.CreateAsync(
                to: new PhoneNumber(toPhoneNumber),
                from: new PhoneNumber(_fromPhone),
                body: message
            );

            return $"✅ SMS Sent. SID: {result.Sid}";
        }

        public async Task<string> SendWhatsappMessage(string toWhatsAppNumber, string message)
        {
            var result = await MessageResource.CreateAsync(
                to: new PhoneNumber($"whatsapp:{toWhatsAppNumber}"),
                from: new PhoneNumber($"whatsapp:{_whatsappNumber}"),
                body: message
            );
            return $"✅ WhatsApp Message Sent. SID: {result.Sid}";
        }

    }

}
