using HouseRentalDomain.Entities.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseRentalDomain.Entities.Common.CommonEntities;

namespace HouseRentalInfrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyImage> PropertyImages { get; set; }
        public virtual DbSet<RentalRequest> RentalRequests { get; set; }
        public virtual DbSet<CreditTransaction> CreditTransactions { get; set; }
        public virtual DbSet<SubscriptionPlans> SubscriptionPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Property>(e =>
            {
                e.Property(p => p.Price).HasColumnType("decimal(18,2)");
                e.HasOne(p => p.Owner).WithMany(u => u.Properties).HasForeignKey(p => p.OwnerId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<RentalRequest>(e =>
            {
                e.HasOne(r => r.Property).WithMany(p => p.Requests).HasForeignKey(r => r.PropertyId);
                e.HasOne(r => r.Tenant).WithMany(u => u.Requests).HasForeignKey(r => r.TenantId).OnDelete(DeleteBehavior.Restrict);
            });

            // Add any indexes or constraints
            builder.Entity<Property>().HasIndex(p => new {p.PropertyId, p.District, p.Area });
            builder.Entity<RentalRequest>().HasIndex(r => new { r.PropertyId, r.TenantId, r.Status });
            builder.Entity<CreditTransaction>().HasIndex(r => new { r.UserId });
        }
    }
}
