using System.Text.Json.Serialization;

public class CustomerDTO
{
    public int CustomerId { get; set; }


    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public int ShopId { get; set; }
    public string ShopName { get; set; }


    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }
}
