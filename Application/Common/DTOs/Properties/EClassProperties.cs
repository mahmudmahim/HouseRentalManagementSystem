using Microsoft.AspNetCore.Http;
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
            public int PropertyId { get; set; }
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
            public string OwnerId { get; set; }
            public string Status { get; set; } = "Draft";
            public List<PropertyImageDto>? Images { get; set; }
        }

        public class PropertyImageDto
        {
            [Required] 
            public string Url { get; set; } = string.Empty;
            public int SortOrder { get; set; }
        }

        public class PropertyListDto
        {
            public int PropertyId { get; set; }
            public string Title { get; set; } = "";
            public decimal Price { get; set; }
            public int Sqft { get; set; }
            public string District { get; set; } = "";
            public string Area { get; set; } = "";
            public string Address { get; set; } = "";
            public string? FirstImageUrl { get; set; }
        }

        public class FileUploadDto
        {
            public IFormFile File { get; set; }
        }

        public class PropertyEditDto
        {
            public int PropertyId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int Sqft { get; set; }
            public int BedRooms { get; set; }
            public int Balcony { get; set; }
            public int WashRooms { get; set; }
            public string Address { get; set; }
            public string Area { get; set; }
            public string District { get; set; } = string.Empty;

            public List<PropertyImageEditDto> Images { get; set; } = new();
        }

        public class PropertyImageEditDto
        {
            public int ImageId { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
