using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentalApplication.Common.DTOs.Properties
{
    public class EClassProperties
    {
        public class PropertyCreateDto
        {
            [Required] 
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            [Range(0, double.MaxValue)] 
            public decimal Price { get; set; }
            public int Sqft { get; set; }
            public int Bedrooms { get; set; }
            public int Balcony { get; set; }
            public int Washroom { get; set; }
            [Required] 
            public string Address { get; set; } = string.Empty;
            public string? Area { get; set; }
            [Required] 
            public string District { get; set; } = string.Empty;
            public string OwnerId { get; set; } = string.Empty;
            public string Status { get; set; } = "Draft";
            public List<PropertyImageDto>? Images { get; set; }
        }

        public class PropertyImageDto
        {
            [Required] 
            public string Url { get; set; } = string.Empty;
            public int SortOrder { get; set; }
        }
    }
}
