using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Indentity.ViewModels;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
}