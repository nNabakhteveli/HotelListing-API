using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models;

public class UserLoginDTO
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [StringLength(15, ErrorMessage = "Maximum length for a password is 15 characters")]
    public string Password { get; set; }
}