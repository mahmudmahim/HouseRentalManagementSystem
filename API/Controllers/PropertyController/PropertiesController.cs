using HouseRentalApplication.Common.Interfaces.Properties;
using Microsoft.AspNetCore.Mvc;
using static HouseRentalApplication.Common.DTOs.Properties.EClassProperties;

namespace HouseRentalAPI.Controllers.PropertyController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : Controller
    {
        private readonly IPropertyService _service;

        public PropertiesController(IPropertyService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PropertyCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var p = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = p.PropertyId }, p);
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
            return p == null ? NotFound() : Ok(p);
        }
    }
}
