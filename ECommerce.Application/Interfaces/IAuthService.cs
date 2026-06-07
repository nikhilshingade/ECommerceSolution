using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTOs.Auth;

namespace ECommerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDTO dto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO dto);
    }
}
