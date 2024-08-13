using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Indentity.ViewModels;

public class UserViewModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}