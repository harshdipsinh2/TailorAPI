using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TailorAPI.Models;

// Enum for UserStatus
public enum UserStatus
{
    Available,
    Busy
}

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string MobileNo { get; set; }
    public string PasswordHash { get; set; }
    public string Address { get; set; }

    public int RoleID { get; set; }
    [ForeignKey("RoleID")]
    public Role Role { get; set; }

    [Column(TypeName = "nvarchar(10)")] // ✅ Stored as string in SQL
    [JsonConverter(typeof(JsonStringEnumConverter))] // ✅ Displayed as string in JSON
    public UserStatus UserStatus { get; set; } = UserStatus.Available; // Default value

    public bool IsDeleted { get; set; } = false;
    //public string Password { get; internal set; }
}
