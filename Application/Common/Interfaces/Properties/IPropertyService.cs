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
        Task<Property?> GetPropertyById(int id);
        Task<List<PropertyListDto>> GetPropertiesByOwner(string ownerId);
        Task<List<PropertyCreateDto>> GetAllPropertiesAsync();
        Task<bool> UpdateAsync(PropertyEditDto dto);
        Task DeleteAsync(Property property);
    }

    public interface IImageService
    {
        Task<ImageSaveResult> SaveAsync(IFormFile file);
        Task DeleteAsync(string url);
    }

    
}
