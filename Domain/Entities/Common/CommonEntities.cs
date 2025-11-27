using HouseRentalDomain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentalDomain.Entities.Common
{
    public class CommonEntities
    {
        public enum RequestStatus { Pending, Approved, Rejected }
        public enum TransactionType { Credit, Debit }
        public enum PropertyStatus { Draft, Published, Paused, Archived }

        public class Property
        {
           
            public int PropertyId { get; set; }
            public string Title { get; set; } = null!;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; } // monthly rent
            public int Sqft { get; set; }
            public int BedRooms { get; set; }
            public int Balcony { get; set; }
            public int Washroom { get; set; }
            public string Address { get; set; } = null!;
            public string? Area { get; set; }
            public string? District { get; set; }

            // owner
            public string OwnerId { get; set; } = null!;
            public ApplicationUser? Owner { get; set; }

            public PropertyStatus Status { get; set; } = PropertyStatus.Published;
            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

            public ICollection<PropertyImage>? Images { get; set; }
            public ICollection<RentalRequest>? Requests { get; set; }
        }

        public class PropertyImage
        {
            public int Id { get; set; }
            public int PropertyId { get; set; }
            public Property? Property { get; set; }
            public string Url { get; set; } = null!;
            public int SortOrder { get; set; }
        }

        public class RentalRequest
        {
            public int Id { get; set; }
            public int PropertyId { get; set; }
            public Property? Property { get; set; }

            public string TenantId { get; set; } = null!;
            public ApplicationUser? Tenant { get; set; }

            public string Message { get; set; } = string.Empty;
            public RequestStatus Status { get; set; } = RequestStatus.Pending;
            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
            public DateTime? ProcessedDate { get; set; }
            public string? ProcessedBy { get; set; } // owner id who processed
            public string? RejectReason { get; set; }
        }

        public class CreditTransaction
        {
            public int Id { get; set; }
            public string UserId { get; set; } = null!;
            public ApplicationUser? User { get; set; }
            public int TransactionAmount { get; set; }
            public int TotalCreditBalance { get; set; }
            public TransactionType Type { get; set; }
            public string? Reason { get; set; }
            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        }

        public class SubscriptionPlans
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public ApplicationUser? User { get; set; }
            public string? SubscriptionName { get; set; }
            public int PlanAmount { get; set; }
            public int CreaditBalance { get; set; }
            public string? Description { get; set; }

        }
    }
}
