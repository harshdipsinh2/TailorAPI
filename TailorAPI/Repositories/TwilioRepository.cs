using TailorAPI.Models;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTO.ResponseDTO;

using System;

namespace TailorAPI.Repositories
{
    public class TwilioRepository
    {
        private readonly TailorDbContext _context;

        public TwilioRepository(TailorDbContext context)
        {
            _context = context;
        }

        public async Task SaveSmsAsync(TwilioRequestDTO dto, string message)
        {
            var sms = new TwilioSms
            {
                OrderID = dto.OrderID,
                SmsType = dto.SmsType,
                SentAt = dto.SentAt,
                Message = message
            };

            _context.TwilioSms.Add(sms);
            await _context.SaveChangesAsync();
        }
    }
}
