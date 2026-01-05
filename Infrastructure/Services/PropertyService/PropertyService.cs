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

        public async Task<Property?> GetPropertyById(int id)
        {
            var getProperty = await _db.Properties.Include(x => x.Images)
                                       .FirstOrDefaultAsync(x => x.PropertyId == id);
            return getProperty;
        }

        public async Task<List<PropertyListDto>> GetPropertiesByOwner(string ownerId)
        {
            var properties = await _db.Properties.Where(p => p.OwnerId == ownerId).Include(x => x.Images).Select(i => new PropertyListDto
            {
                PropertyId = i.PropertyId,
                Title = i.Title,
                Price = i.Price,
                Sqft = i.Sqft,
                District = i.District ?? "",
                Area = i.Area ?? "",
                Address = i.Address,
                FirstImageUrl = i.Images.OrderBy(i => i.SortOrder).Select(i => i.Url).FirstOrDefault()
            }).ToListAsync();

            return properties;
        }

        public async Task<List<PropertyCreateDto>> GetAllPropertiesAsync()
        {
            var property = await _db.Properties.AsNoTracking().Select(p => new PropertyCreateDto
            {
                PropertyId = p.PropertyId,
                Description = p.Description ?? string.Empty,
                Title = p.Title,
                Price = p.Price,
                Sqft = p.Sqft,
                Bedrooms = p.BedRooms,
                Balcony = p.Balcony,
                Washroom = p.Washroom,
                District = p.District ?? "",
                Area = p.Area ?? "",
                Address = p.Address,
                Images = p.Images!.OrderBy(i => i.SortOrder).Select(i => new PropertyImageDto
                {
                    Url = i.Url,
                    SortOrder = i.SortOrder
                }).ToList()
            }).ToListAsync();

            return property;
        }


        public async Task<bool> UpdateAsync(PropertyEditDto dto)
        {
            var p = await _db.Properties
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.PropertyId == dto.PropertyId);

            if (p == null) return false;

            p.Title = dto.Title;
            p.Description = dto.Description;
            p.Price = dto.Price;
            p.Sqft = dto.Sqft;
            p.BedRooms = dto.BedRooms;
            p.Balcony = dto.Balcony;
            p.Washroom = dto.WashRooms;
            p.Address = dto.Address;
            p.Area = dto.Area;
            p.District = dto.District;

            await _db.SaveChangesAsync();
            return true;
        }
        public async Task DeleteAsync(Property property)
        {
            _db.Properties.Remove(property);
            await _db.SaveChangesAsync();
        }
    }
}
