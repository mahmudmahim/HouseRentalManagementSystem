using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentalApplication.Common.DTOs.Auth
{
    public class RegisterDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? NIDNo { get; set; }
        public string? Password { get; set; }
        public bool IsOwner { get; set; }
        public bool IsAdmin { get; set; }

    }
}
