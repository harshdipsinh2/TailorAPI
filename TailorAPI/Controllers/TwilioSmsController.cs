// File: Controllers/TwilioSmsController.cs

using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Services;
using TailorAPI.Repositories;

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

        [HttpPost("send")]
        public async Task<IActionResult> SendSms([FromQuery] string phoneNumber, [FromBody] TwilioRequestDTO dto)
        {
            var messageBody = $"[Order {dto.OrderID}] Notification - Type: {dto.SmsType}";

            var sendResult = await _twilioService.SendSmsAsync(phoneNumber, messageBody);

            if (sendResult.Contains("SMS Sent"))
            {
                await _twilioRepository.SaveSmsAsync(dto, messageBody);
                return Ok(new { status = "success", result = sendResult });
            }

            return BadRequest(new { status = "failed", result = sendResult });
        }

        [HttpPost("SendWhatsapp")]
        public async Task <IActionResult> SendWhatsapp([FromQuery] string phoneNumber , [FromBody] TwilioRequestDTO dto)

        {

            var messageBody = $"[Order {dto.OrderID}] Notification - Type: {dto.SmsType}";
            var sendResult = await _twilioService.SendWhatsappMessage(phoneNumber, messageBody);

            if (sendResult.Contains("SMS Sent"))
            {
                await _twilioRepository.SaveSmsAsync(dto, messageBody);
                return Ok(new { status = "success", result = sendResult });
            }

            return BadRequest(new { status = "failed", result = sendResult });


        }



    }
}
