using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HouseRentalWeb.Pages.Dashboard
{
    public class TenantDashboardModel : PageModel
    {
        public string TenantName = "Khairuzzaman";
        public List<PropertyModel> AllProperties { get; set; }
        public List<RequestModel> MyRequests { get; set; }

        public void OnGet()
        {
            // Mock Data
            AllProperties = new List<PropertyModel>
        {
            new() { Title="Modern Apartment in Banani", Price=500, Size=1200, Location="Banani, Dhaka", ImageUrl="/images/home01.jpg" },
            new() { Title="Studio in Dhanmondi", Price=350, Size=900, Location="Dhanmondi, Dhaka", ImageUrl="/images/home02.jpeg" },
            new() { Title="Luxury Condo in Gulshan", Price=800, Size=1500, Location="Gulshan, Dhaka", ImageUrl="/images/home03.jpg" }
        };

            MyRequests = new List<RequestModel>
        {
            new() { PropertyTitle="Studio in Dhanmondi", Location="Dhanmondi, Dhaka", Status="Pending" }
        };
        }

        public class PropertyModel
        {
            public string Title { get; set; }
            public decimal Price { get; set; }
            public int Size { get; set; }
            public string Location { get; set; }
            public string ImageUrl { get; set; }
        }

        public class RequestModel
        {
            public string PropertyTitle { get; set; }
            public string Location { get; set; }
            public string Status { get; set; }
        }
    }
}
