using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TransportManagementSystem.Models
{
    public class Passenger
    {
        [Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int PsngrId { get; set; }


        [Required]
        [StringLength(50)]
        public string PsngrName{ get; set; } = default!;


        [Required]
        [StringLength(50)]
        public string PsngrContact { get; set; } = default!;


        public string ImageUrl { get; set; } = ""!;


        public string Seatno { get; set; } = default!;

        [ForeignKey("Trip")]
        public int? TripId { get; set; }  

        public Trip? Trip { get; set; } = default!;

    }
}