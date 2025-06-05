using System.Threading.Tasks;
using TailorAPI.DTO.ResponseDTO;
using TailorAPI.Models;

namespace TailorAPI.Services
{
    public interface ITwilioService
    {
        Task<string> SendSmsAsync(string toPhoneNumber, string message);

        Task<string> SendWhatsappTemplateMessage(string toWhatsAppNumber, SmsType smsType, int orderId);

        Task<IEnumerable<TwilioSmsResponseDTO>> GetAllAsync();
}
}