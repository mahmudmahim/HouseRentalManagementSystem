using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace HouseRentalWeb.Pages.Dashboard
{
    public class OwnerDashboardModel : PageModel
    {
        public int CreditBalance { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int PendingDelta { get; set; }

        public string SearchQuery { get; set; } = "";
        public string FilterStatus { get; set; } = "";

        public List<RequestItem> Requests { get; set; } = new();
        public List<PropertyItem> Properties { get; set; } = new();

        // For the detail drawer + action
        public RequestItem ActiveRequest { get; set; }
        public int ActionRequestId { get; set; }
        public string RejectReason { get; set; } = "";
        public void OnGet()
        {
            // sample credit info
            CreditBalance = 120;
            PendingRequests = 6;
            ApprovedRequests = 18;
            RejectedRequests = 4;
            PendingDelta = 8;


            Properties = new List<PropertyItem>
        {
            new PropertyItem { Id = 1, Title="2BHK in Gulshan", Location="Gulshan, Dhaka", Price="35,000", RequestCount=3, ImageUrl="/images/home01.jpg" },
            new PropertyItem { Id = 2, Title="Apartment in Banani", Location="Banani, Dhaka", Price="28,000", RequestCount=5, ImageUrl="/images/home02.jpeg" },
            new PropertyItem { Id = 3, Title="Studio Dhanmondi", Location="Dhanmondi, Dhaka", Price="22,000", RequestCount=0, ImageUrl="/images/home03.jpg" }
        };

            // sample requests
            Requests = new List<RequestItem>
        {
            new RequestItem { Id = 101, TenantName="Afnan", PropertyTitle="2BHK in Gulshan", Location="Gulshan", Contact="017XXXXXXXX", Score=88, Status="Pending", Message="Interested from next month." },
            new RequestItem { Id = 102, TenantName="Rahul", PropertyTitle="Apartment in Banani", Location="Banani", Contact="019XXXXXXXX", Score=72, Status="Pending", Message="Can we view tomorrow?" },
            new RequestItem { Id = 103, TenantName="Maya", PropertyTitle="Studio Dhanmondi", Location="Dhanmondi", Contact="016XXXXXXXX", Score=95, Status="Approved", Message="Confirmed via call." }
        };

            // Default ActiveRequest null
            ActiveRequest = null;
        }

        // called by button click (server side)
        public void SetActionRequestId(int id)
        {
            ActionRequestId = id;
        }

        public void OpenDetails(int id)
        {
            ActiveRequest = Requests.FirstOrDefault(r => r.Id == id);
        }

        public void ConfirmApprove()
        {
            var r = Requests.FirstOrDefault(x => x.Id == ActionRequestId);
            if (r != null) r.Status = "Approved";
            // update stats
            ApprovedRequests++;
            PendingRequests = Requests.Count(x => x.Status == "Pending");
        }

        public void ConfirmReject()
        {
            var r = Requests.FirstOrDefault(x => x.Id == ActionRequestId);
            if (r != null) r.Status = "Rejected";
            // record RejectReason somewhere (demo only)
            RejectedRequests++;
            PendingRequests = Requests.Count(x => x.Status == "Pending");
        }

        public void ResetFilters()
        {
            SearchQuery = "";
            FilterStatus = "";
        }

        // small helpers for UI badges (if you want to move to helper, ok)
        public string GetScoreBadge(int score)
        {
            if (score >= 85) return "bg-success text-white";
            if (score >= 60) return "bg-warning text-dark";
            return "bg-danger text-white";
        }

        public string GetStatusBadge(string status)
        {
            return status switch
            {
                "Pending" => "bg-secondary text-white",
                "Approved" => "bg-success text-white",
                "Rejected" => "bg-danger text-white",
                _ => "bg-light text-dark"
            };
        }

        public class RequestItem
        {
            public int Id { get; set; }
            public string TenantName { get; set; }
            public string PropertyTitle { get; set; }
            public string Location { get; set; }
            public string Contact { get; set; }
            public int Score { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }
        }

        public class PropertyItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Location { get; set; }
            public string Price { get; set; }
            public int RequestCount { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
