using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models;

public class UserRegistrationDTO : UserLoginDTO
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
}