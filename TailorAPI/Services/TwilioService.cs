using Microsoft.Extensions.Configuration;
using TailorAPI.DTO.ResponseDTO;
using TailorAPI.Repositories;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;
using System.Text.Json;

namespace TailorAPI.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhone;
        private readonly string _whatsappNumber;
        private readonly string _delayedMessageSid;
        private readonly string _preCompletionMessageSid;
        private readonly string _completionMessageSid;
        private readonly TailorDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TwilioService(IConfiguration configuration, TailorDbContext context,IHttpContextAccessor httpContextAccessor)
        {
            _accountSid = configuration["Twilio:AccountSid"] ?? throw new ArgumentNullException("Twilio:AccountSid");
            _authToken = configuration["Twilio:AuthToken"] ?? throw new ArgumentNullException("Twilio:AuthToken");
            _fromPhone = configuration["Twilio:FromPhoneNumber"] ?? throw new ArgumentNullException("Twilio:FromPhoneNumber");
            _whatsappNumber = configuration["Twilio:WhatsAppNumber"] ?? throw new ArgumentNullException("Twilio:WhatsAppNumber");
            _delayedMessageSid = configuration["Twilio:DelayedMessageSid"] ?? throw new ArgumentNullException("Twilio:DelayedMessageSid");
            _preCompletionMessageSid = configuration["Twilio:PreCompleteMessageSid"] ?? throw new ArgumentNullException("Twilio:PreCompleteMessageSid");
            _completionMessageSid = configuration["Twilio:MessageSid"] ?? throw new ArgumentNullException("Twilio:MessageSid");

            _context = context; ///twilio done 
            _httpContextAccessor = httpContextAccessor;

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

        public async Task<string> SendWhatsappTemplateMessage(string toWhatsAppNumber, SmsType smsType, int orderId)
        {
            string templateSid = smsType switch
            {
                SmsType.PreCompletion => _preCompletionMessageSid,
                SmsType.Delayed => _delayedMessageSid,
                SmsType.Completion => _completionMessageSid,
                _ => throw new InvalidOperationException("Invalid SmsType")
            };

            var contentVariables = new
            {
                order_id = orderId.ToString()
            };

            var message = await MessageResource.CreateAsync(
                from: new PhoneNumber($"whatsapp:{_whatsappNumber}"),
                to: new PhoneNumber($"whatsapp:{toWhatsAppNumber}"),
                contentSid: templateSid,
                contentVariables: JsonSerializer.Serialize(contentVariables)
            );

            return message.ErrorCode != null
                ? $"❌ Error: {message.ErrorCode} - {message.ErrorMessage}"
                : $"✅ WhatsApp Template Sent. SID: {message.Sid}";
        }

        public async Task<IEnumerable<TwilioSmsResponseDTO>> GetAllAsync()
        {
            return await _context.TwilioSms
                .Select(sms => new TwilioSmsResponseDTO
                {
                    OrderID = sms.OrderID,
                    Message = sms.Message,
                    SentAt = sms.SentAt,
                    SmsType = sms.SmsType.ToString()
                })
                .ToListAsync();
        }
    }
}
