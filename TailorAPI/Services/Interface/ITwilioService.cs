using System.Threading.Tasks;

namespace TailorAPI.Services
{
    public interface ITwilioService
    {
        Task<string> SendSmsAsync(string toPhoneNumber, string message);

        Task<string> SendWhatsappMessage(string toWhatsAppNumber, string message);
}
}