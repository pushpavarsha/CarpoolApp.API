using CarpoolApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CarpoolApp.API.Data
{
    public class CarpoolDbContext : DbContext
    {
        public CarpoolDbContext(DbContextOptions<CarpoolDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RideOffer> RideOffers { get; set; }
        public DbSet<RideRequest> RideRequests { get; set; }
        public DbSet<RideRequestStatus> RideRequestStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique email constraint
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            // Employee -> Vehicles (one to many)  -- CHANGED to Restrict
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Employee)
                .WithMany(e => e.Vehicles)
                .HasForeignKey(v => v.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee (Driver) -> RideOffers (one to many)
            modelBuilder.Entity<RideOffer>()
                .HasOne(r => r.Driver)
                .WithMany(e => e.RideOffers)
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vehicle -> RideOffers (one to many)
            modelBuilder.Entity<RideOffer>()
                .HasOne(r => r.Vehicle)
                .WithMany()
                .HasForeignKey(r => r.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // RideOffer -> RideRequests (one to many)
            modelBuilder.Entity<RideRequest>()
                .HasOne(rr => rr.RideOffer)
                .WithMany(r => r.RideRequests)
                .HasForeignKey(rr => rr.RideOfferId)
                .OnDelete(DeleteBehavior.Cascade);

            // Employee (Requester) -> RideRequests (one to many) -- stays Restrict (SQL Server multi-path rule)
            modelBuilder.Entity<RideRequest>()
                .HasOne(rr => rr.Requester)
                .WithMany(e => e.RideRequests)
                .HasForeignKey(rr => rr.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RideRequest>()
                .HasOne(rr => rr.Status)
                .WithMany(s => s.RideRequests)
                .HasForeignKey(rr => rr.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RideRequestStatus>().HasData(
                new RideRequestStatus { StatusId = 1, StatusName = "Pending" },
                new RideRequestStatus { StatusId = 2, StatusName = "Approved" },
                new RideRequestStatus { StatusId = 3, StatusName = "Rejected" },
                new RideRequestStatus { StatusId = 4, StatusName = "Completed" }
            );
        }
    }
}
