using TailorAPI.Models;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Services.Interface;
using TailorAPI.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Microsoft.AspNetCore.Mvc;

public class OtpVerificationService : IOtpVerificationService
{
    private readonly TailorDbContext _context;
    private readonly UserRepository _userRepository;
    private readonly OtpVerificationRepository _OtpVerificationRepository; // Assume you’ve created this
    private readonly IEmailService _emailService;

    public OtpVerificationService(UserRepository userRepository, OtpVerificationRepository otpVerificationRepository, IEmailService emailService, TailorDbContext context)
    {
        _userRepository = userRepository;
        _OtpVerificationRepository = otpVerificationRepository;
        _emailService = emailService;
        _context = context;
    }

    public async Task<bool> GenerateAndSendOtpAsync([FromQuery]string email )
    {
        try
        {
            // ✅ Step 1: Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            // ✅ Step 2: Generate OTP and expiry
            // ✅ Generate a 6-digit numeric OTP
            string otp = new Random().Next(100000, 999999).ToString();

            // ✅ Set expiry time to 5 minutes from now
            DateTime expiry = DateTime.UtcNow.AddMinutes(5);


            // ✅ Step 3: Save OTP in DB
            await _OtpVerificationRepository.SaveOtpAsync(user.UserID, otp, expiry);

            // ✅ Step 4: Prepare Email Body
            string emailBody = $"Your OTP is {otp}.\n Do not share with anyone ";

            // ✅ Step 5: Send email
            bool isSent = await _emailService.SendEmailAsync(email, "OTP Verification", emailBody);

            // Optional: log to console
            Console.WriteLine(isSent ? "OTP Email sent." : "Failed to send OTP email.");

            return isSent;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending OTP: " + ex.Message);
            return false;
        }

    }


    public async Task<bool> VerifyOtpAsync(OtpVerificationsRequestDTO dto)
    {
        // 1️⃣ Find user by email
        var user = (await _userRepository.GetAllUsersAsync())
                    .FirstOrDefault(u => u.Email == dto.Email);
        if (user == null) return false;

        // 2️⃣ Fetch latest OTP for that user
        var otpRecord = await _OtpVerificationRepository.GetLatestOtpAsync(user.UserID);
        if (otpRecord == null) return false;

        // 3️⃣ Check if OTP is valid and not expired
        if (otpRecord.Otp != dto.Otp || otpRecord.OtpExpiry < DateTime.UtcNow)
            return false;

        user.IsVerified = true;
        await _userRepository.UpdateUserAsync(user);

        // 4️⃣ OTP is valid — delete it from DB
        _context.OtpVerifications.Remove(otpRecord);
        await _context.SaveChangesAsync();

        return true;
    }

}
