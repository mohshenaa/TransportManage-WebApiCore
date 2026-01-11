using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TransportManagementSystem.Models
{
    public class Trip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TripId { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }

        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        //[Required]
        //public string TicketNo { get; set; } = default!;

        [Required]
        public string StartLocation { get; set; } = default!;

        [Required]
        public string Destination { get; set; } = default!;


        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]      
        public DateTime StartDateTime { get; set; } = DateTime.Now;


        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public decimal DistanceKm { get; set; }

        public string Status { get; set; } = "Scheduled";

     

		public string Helper { get; set; } = default!;

        public Driver? Driver { get; set; } = default!;
        public Vehicle? Vehicle { get; set; } = default!;


        //[ForeignKey("Passenger")]
       // public int? PsngrId { get; set; }

        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
    }
}