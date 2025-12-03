using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseRentalDomain.Entities.Common.CommonEntities;

namespace HouseRentalDomain.Entities.Auth
{
    public class ApplicationUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? NIDNo { get; set; }
        public string? Password { get; set; }
        public bool IsOwner { get; set; } 
        public bool IsAdmin { get; set; }


        // Navigation (optional)
        public ICollection<Property>? Properties { get; set; }
        public ICollection<RentalRequest>? Requests { get; set; }


    }
}
