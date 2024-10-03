using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Dtos;

public class UserRegistrationDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}