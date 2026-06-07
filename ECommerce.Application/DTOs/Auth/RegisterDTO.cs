using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs.Auth;
public class RegisterDTO
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
