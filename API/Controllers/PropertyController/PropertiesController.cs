using HouseRentalApplication.Common.Interfaces.Properties;
using Microsoft.AspNetCore.Mvc;
using static HouseRentalApplication.Common.DTOs.Properties.EClassProperties;

namespace HouseRentalAPI.Controllers.PropertyController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _service;
        private readonly IImageService _imageService;

        public PropertiesController(IPropertyService service, IImageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        [HttpPost("upload-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] FileUploadDto dto)
        {
            if (dto.File == null)
                return BadRequest(new { error = "No file provided" });

            if (!dto.File.ContentType.StartsWith("image/"))
                return BadRequest(new { error = "Invalid file type" });

            var result = await _imageService.SaveAsync(dto.File);

            return Ok(new { url = result.Url, id = result.Id });
        }

        [HttpPost("createproperty")]
        public async Task<IActionResult> Create(PropertyCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var p = await _service.CreateAsync(dto);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _service.GetByIdAsync(id);

            if (p == null)
                return NotFound();

            var dto = new PropertyCreateDto
            {
                PropertyId = p.PropertyId,
                Title = p.Title,
                Description = p.Description,
                Bedrooms = p.BedRooms,
                Balcony = p.Balcony,
                Washroom = p.Washroom,
                Area = p.Area,
                Address = p.Address,
                District = p.District,
                OwnerId = p.OwnerId,
                Price = p.Price,
                Sqft = p.Sqft,
                Images = p.Images.Select(img => new PropertyImageDto
                {
                    SortOrder = img.SortOrder,
                    Url = img.Url
                }).ToList()
            };

            return Ok(dto);
        }
    }
}
