﻿namespace BusinessLogic.Models;

public class LoginRequestDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}