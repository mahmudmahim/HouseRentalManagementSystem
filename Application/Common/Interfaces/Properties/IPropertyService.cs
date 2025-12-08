using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseRentalApplication.Common.DTOs.Properties.EClassProperties;
using static HouseRentalDomain.Entities.Common.CommonEntities;
using Microsoft.AspNetCore.Http;

namespace HouseRentalApplication.Common.Interfaces.Properties
{
    public interface IPropertyService
    {
        Task<Property> CreateAsync(PropertyCreateDto dto);
        Task<Property?> GetByIdAsync(int id);
    }

    public interface IImageService
    {
        Task<ImageSaveResult> SaveAsync(IFormFile file);
        Task DeleteAsync(string url);
    }

    
}
