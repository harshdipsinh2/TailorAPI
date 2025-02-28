using System.Text.Json.Serialization;

namespace TailorAPI.DTO
{
    public class EmployeeDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Role { get; set; } // Role assigned during registration
        public decimal Salary { get; set; }
        public int Attendance { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmployeeStatus Status { get; set; }
        public int? CustomerID { get; set; } // Assigned Customer

        [JsonIgnore] // Hides MeasurementID from API response
        public int? MeasurementID { get; set; }
    }
}