using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentalApplication.Common.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public List<AuthResponseDTO>? Data { get; set; }
    }
}
