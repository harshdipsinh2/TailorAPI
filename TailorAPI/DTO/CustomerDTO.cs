using System.Text.Json.Serialization;

public class CustomerDTO
{

    public int CustomerId { get; set; }  // ✅ Match this with Customer model
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}
