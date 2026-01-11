using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using TransportManagementSystem.Models;

namespace TransportManage_WebApiCore.Data
{
    public class TransportDb : IdentityDbContext<AppUser>
    {
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public TransportDb(DbContextOptions<TransportDb> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder build)
        {
            base.OnModelCreating(build);

            build.Entity<Vehicle>().HasData(
           new Vehicle { ViclId = 1, ViclNum = "DHA-11-1001", ViclModel = "Toyota Corolla", Capacity = 4, Status = "Available" },
           new Vehicle { ViclId = 2, ViclNum = "DHA-11-1002", ViclModel = "Nissan Sunny", Capacity = 4, Status = "Booked" },
           new Vehicle { ViclId = 3, ViclNum = "CTG-22-2001", ViclModel = "Toyota Hiace Microbus", Capacity = 12, Status = "Available" },
           new Vehicle { ViclId = 4, ViclNum = "CTG-22-2002", ViclModel = "Nissan Urvan Microbus", Capacity = 14, Status = "Under Maintenance" },
           new Vehicle { ViclId = 5, ViclNum = "RAJ-33-3001", ViclModel = "Mitsubishi L300 Minibus", Capacity = 16, Status = "Available" },
           new Vehicle { ViclId = 6, ViclNum = "RAJ-33-3002", ViclModel = "Toyota Coaster Minibus", Capacity = 18, Status = "Booked" },
           new Vehicle { ViclId = 7, ViclNum = "SYL-44-4001", ViclModel = "Honda Grace", Capacity = 4, Status = "Available" }
       );


            build.Entity<Driver>().HasData(
            new Driver { DriId = 1, DriName = "Sakil", LicenseNum = "LIC-DHA-12345", Contact = "01711122334", ImageUrl = "", IsAvailable = true },
            new Driver { DriId = 2, DriName = "Rakib", LicenseNum = "LIC-DHA-56789", Contact = "01822233445", ImageUrl = "", IsAvailable = false },
            new Driver { DriId = 3, DriName = "Rashed", LicenseNum = "LIC-CTG-11223", Contact = "01933344556", ImageUrl = "", IsAvailable = true },
            new Driver { DriId = 4, DriName = "Jewel", LicenseNum = "LIC-RAJ-44556", Contact = "01644455667", ImageUrl = "", IsAvailable = true },
            new Driver { DriId = 5, DriName = "Nesar", LicenseNum = "LIC-SYL-77889", Contact = "01555566778", ImageUrl = "", IsAvailable = false },
            new Driver { DriId = 6, DriName = "Nayeem", LicenseNum = "LIC-BAR-99001", Contact = "01766677889", ImageUrl = "", IsAvailable = true }
        );
            build.Entity<Trip>().HasData(
    new Trip
    {
        TripId = 1,
        VehicleId = 1,
        DriverId = 2,
        StartLocation = "Dhaka",
        Destination = "Ctg",
        StartDateTime = DateTime.Parse("2025-12-07T05:30:35.070Z"),
        EndDate = DateTime.Parse("2025-12-07T05:30:35.070Z"),
        DistanceKm = 211,
        Status = "Cancelled",
        Helper = "Nun"
    }
);


            build.Entity<Passenger>().HasData(
          new Passenger
          {
              PsngrId = 1,
              TripId = 1,
              PsngrName = "Meem",
              PsngrContact = "01612223456",
              ImageUrl = "",
              Seatno = "A4"
          },
          new Passenger
          {
              PsngrId = 2,
              TripId = 1,
              PsngrName = "Mahira",
              PsngrContact = "01612223456",
              ImageUrl = "",
              Seatno = "A3"
          }
      );


            build.Entity<Trip>()
                .HasMany(t => t.Passengers)
                .WithOne(p => p.Trip)
                .HasForeignKey(p => p.TripId)
                .OnDelete(DeleteBehavior.Cascade);

        //    build.Entity<Passenger>()
        //.HasIndex(p => new { p.TripId, p.Seatno })
        //.IsUnique()
        //.HasDatabaseName("Unique_Seat_Per_Trip");


  

        }

    }

    public class AppUser : IdentityUser
    {
        [DataType(DataType.ImageUrl)]
        public string? PhotoPath { get; set; }
    }
    public class UserRole
    {
        public string UserName { get; set; } = default!;

        public string Role { get; set; } = default!;
    }
    public class UserRoleAssignDto
    {
        [Required]
        public string UserName { get; set; } = default!;
        public IList<string> Roles { get; set; } = [];
    }

    public class UserDto
    {
        [Required]
        public string? UserName { get; set; }


        [Required]
        [EmailAddress]
        public string? Email { get; set; }


        [Required]
        public string? Password { get; set; }
    }
    public class LoginDto
    {
        [Required]
        public string? UserName { get; set; }



        [Required]
        public string? Password { get; set; }
    }
}
