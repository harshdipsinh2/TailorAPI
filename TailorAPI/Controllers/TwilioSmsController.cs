using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Services;
using TailorAPI.Repositories;
using TailorAPI.Models;

namespace TailorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioSmsController : ControllerBase
    {
        private readonly ITwilioService _twilioService;
        private readonly TwilioRepository _twilioRepository;

        public TwilioSmsController(ITwilioService twilioService, TwilioRepository twilioRepository)
        {
            _twilioService = twilioService;
            _twilioRepository = twilioRepository;
        }
        // File: Controllers/TwilioSmsController.cs

        [HttpPost("send")]
        [Authorize(Roles = "Admin.Manager")]
        public async Task<IActionResult> SendSms([FromQuery] string phoneNumber, [FromBody] TwilioRequestDTO dto)
        {
            string messageBody = dto.SmsType switch
            {
                SmsType.PreCompletion => $"[Order {dto.OrderID}] Reminder: Your order is  ready before the completion date , please visit us or contact for delivery arrangements",
                SmsType.Completion => $"[Order {dto.OrderID}] Good News: Your order is ready for pickup/delivery! Please visit us or contact for delivery arrangements.",
                SmsType.Delayed => $"[Order {dto.OrderID}] Update: We're experiencing slight delays with your order. We appreciate your patience and will update you soon.",
                _ => $"[Order {dto.OrderID}] Notification - Type: {dto.SmsType}"
            };

            var sendResult = await _twilioService.SendSmsAsync(phoneNumber, messageBody);

            if (sendResult.Contains("SMS Sent"))
            {
                await _twilioRepository.SaveSmsAsync(dto, messageBody);
                return Ok(new { status = "success", result = sendResult });
            }

            return BadRequest(new { status = "failed", result = sendResult });
        }
        [HttpPost("SendWhatsapp")]
        public async Task<IActionResult> SendWhatsapp([FromQuery] string phoneNumber, [FromBody] TwilioRequestDTO dto)
        {
            var messageBody = $"[Order {dto.OrderID}] Notification - Type: {dto.SmsType}";
            var sendResult = await _twilioService.SendWhatsappMessage(phoneNumber, messageBody);

            if (sendResult.Contains("Sent"))
            {
                await _twilioRepository.SaveSmsAsync(dto, messageBody);
                return Ok(new { status = "success", result = sendResult });
            }

            return BadRequest(new { status = "failed", result = sendResult });
        }
            


    }
}
