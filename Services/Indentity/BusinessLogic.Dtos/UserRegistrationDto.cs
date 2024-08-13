using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Dtos;

public class UserRegistrationDto
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}