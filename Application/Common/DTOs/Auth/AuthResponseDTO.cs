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
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string? Message { get; set; }
    }

    public class UniqueCheckRequestDTO
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? NidNo { get; set; }
    }

    public class UniqueCheckResponseDTO
    {
        public bool EmailExists { get; set; }
        public bool PhoneExists { get; set; }
        public bool NidExists { get; set; }
        public string? Message { get; set; }
    }

}
