using TailorAPI.DTO;
using TailorAPI.Repositories;
using TailorAPI.Services.Interface;

public class AdminService : IAdminService
{
    private readonly AdminRepository _adminRepository;

    public AdminService(AdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    // Get All Admins
    public async Task<List<UserResponseDto>> GetAllAdminsAsync()
    {
        var admins = await _adminRepository.GetAllAdminsAsync();

        return admins.Select(u => new UserResponseDto
        {
            Name = u.Name,
            Email = u.Email,
            MobileNo = u.MobileNo,
            Address = u.Address,
            RoleName = "Admin", // Since it's always an Admin
            UserStatus = u.UserStatus.ToString() // Convert Enum to string
        }).ToList();
    }

    // Get Admin by ID
    public async Task<UserResponseDto?> GetAdminByIdAsync(int adminId)
    {
        var admin = await _adminRepository.GetAdminByIdAsync(adminId);
        if (admin == null) return null;

        return new UserResponseDto
        {
            Name = admin.Name,
            Email = admin.Email,
            MobileNo = admin.MobileNo,
            Address = admin.Address,
            RoleName = "Admin",
            UserStatus = admin.UserStatus.ToString() // Convert Enum to string
        };
    }

    // Register New Admin
    public async Task<bool> RegisterAdminAsync(UserRequestDto adminDto)
    {
        var newAdmin = new User
        {
            Name = adminDto.Name,
            Email = adminDto.Email,
            MobileNo = adminDto.MobileNo,
            Address = adminDto.Address,
            Password = adminDto.Password, // Consider hashing this before saving
            RoleID = 1, // Admin Role ID
            UserStatus = UserStatus.Available // Use Enum for UserStatus
        };

        await _adminRepository.AddAdminAsync(newAdmin);
        return true;
    }

    // Update Admin
    public async Task<bool> UpdateAdminAsync(int adminId, UserRequestDto updatedAdminDto)
    {
        var admin = await _adminRepository.GetAdminByIdAsync(adminId);
        if (admin == null) return false;

        admin.Name = updatedAdminDto.Name;
        admin.Email = updatedAdminDto.Email;
        admin.MobileNo = updatedAdminDto.MobileNo;
        admin.Address = updatedAdminDto.Address;
        admin.Password = updatedAdminDto.Password; // Consider hashing this too

        await _adminRepository.UpdateAdminAsync(admin);
        return true;
    }

    // Delete Admin
    public async Task<bool> DeleteAdminAsync(int adminId)
    {
        await _adminRepository.DeleteAdminAsync(adminId);
        return true;
    }
}
