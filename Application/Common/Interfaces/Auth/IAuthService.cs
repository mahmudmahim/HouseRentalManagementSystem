using HouseRentalApplication.Common.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentalApplication.Common.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO model);
        Task<AuthResponseDTO> LoginAsync(LoginDTO model);
        Task<UniqueCheckResponseDTO> CheckUniqueFieldsAsync(UniqueCheckRequestDTO dto);

        Task<bool> AssignRoleAsync(string userId, string role);
    }
}
