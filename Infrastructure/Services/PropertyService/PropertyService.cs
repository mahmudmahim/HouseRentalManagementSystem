using HouseRentalApplication.Common.DTOs.Auth;
using HouseRentalApplication.Common.Interfaces.Properties;
using HouseRentalInfrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseRentalApplication.Common.DTOs.Properties.EClassProperties;
using static HouseRentalDomain.Entities.Common.CommonEntities;

namespace HouseRentalInfrastructure.Services.PropertyService
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _db;
        private readonly IImageService _imageService;

        public PropertyService(ApplicationDbContext db, IImageService imageService)
        {
            _db = db;
            _imageService = imageService;
        }

        public async Task<Property> CreateAsync(PropertyCreateDto dto)
        {
            try
            {
                // business rules
                if (dto.Price <= 0)
                    throw new ArgumentException("Price must be greater than zero");

                if (string.IsNullOrWhiteSpace(dto.Title))
                    throw new ArgumentException("Title is required");

                var p = new Property
                {
                    Title = dto.Title,
                    Description = dto.Description ?? string.Empty,
                    Price = dto.Price,
                    Sqft = dto.Sqft,
                    BedRooms = dto.Bedrooms,
                    Balcony = dto.Balcony,
                    Washroom = dto.Washroom,
                    Address = dto.Address,
                    Area = dto.Area,
                    District = dto.District,
                    OwnerId = dto.OwnerId,
                    Status = Enum.TryParse<PropertyStatus>(dto.Status, out var s)
                        ? s : PropertyStatus.Draft,
                    CreatedDate = DateTime.UtcNow
                };

                _db.Properties.Add(p);
                await _db.SaveChangesAsync();

                if (dto.Images != null)
                {
                    int order = 0;
                    foreach (var i in dto.Images.OrderBy(x => x.SortOrder))
                    {
                        _db.PropertyImages.Add(new PropertyImage
                        {
                            PropertyId = p.PropertyId,
                            Url = i.Url,
                            SortOrder = order++
                        });
                    }

                    await _db.SaveChangesAsync();
                }

                return p;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
           
        }

        public async Task<Property?> GetByIdAsync(int id)
        {
            var getProperty = await _db.Properties.Include(x => x.Images)
                                       .FirstOrDefaultAsync(x => x.PropertyId == id);
            return getProperty;
        }
    }
}
