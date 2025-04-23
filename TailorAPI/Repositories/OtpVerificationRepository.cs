using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;

namespace TailorAPI.Repositories
{
    public class OtpVerificationRepository
    {
        private readonly TailorDbContext _context;

        public OtpVerificationRepository(TailorDbContext context)
        {
            _context = context;
        }

        public async Task SaveOtpAsync(int userId, string otp, DateTime expiry)
        {
            var otpVerification = new OtpVerification
            {
                UserID = userId,
                Otp = otp,
                OtpExpiry = expiry
            };

            _context.OtpVerifications.Add(otpVerification);
            await _context.SaveChangesAsync();
        }


        public async Task<OtpVerification?> GetLatestOtpAsync(int userId)
        {
            return await _context.OtpVerifications
                .Where(o => o.UserID == userId)
                .OrderByDescending(o => o.OtpExpiry)
                .FirstOrDefaultAsync();
        }
    }
}
