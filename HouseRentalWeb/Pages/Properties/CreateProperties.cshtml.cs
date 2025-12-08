using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HouseRentalWeb.Pages.Properties
{
    public class CreatePropertiesModel : PageModel
    {
        public string OwnerId { get; private set; } = "";

        public void OnGet()
        {
            OwnerId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";
        }
    }
}
