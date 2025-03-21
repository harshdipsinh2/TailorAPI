using TailorAPI.DTO;

namespace TailorAPI.Services.Interface
{
    public interface IAdminService
    {
        Task<List<UserResponseDto>> GetAllAdminsAsync();
        Task<UserResponseDto?> GetAdminByIdAsync(int adminId);
        Task<bool> RegisterAdminAsync(UserRequestDto adminDto);
        Task<bool> UpdateAdminAsync(int adminId, UserRequestDto updatedAdminDto);
        Task<bool> DeleteAdminAsync(int adminId);
    }
}
