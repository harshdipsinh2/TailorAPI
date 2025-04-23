using TailorAPI.DTO.RequestDTO;

namespace TailorAPI.Services.Interface

{ 
public interface IOtpVerificationService
{
    Task<bool> GenerateAndSendOtpAsync(string email );
    Task<bool> VerifyOtpAsync(OtpVerificationsRequestDTO dto);
}


}