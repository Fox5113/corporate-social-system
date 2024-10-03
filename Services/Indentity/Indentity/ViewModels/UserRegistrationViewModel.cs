using System.ComponentModel.DataAnnotations;

namespace Indentity.ViewModels;

public class UserRegistrationViewModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}