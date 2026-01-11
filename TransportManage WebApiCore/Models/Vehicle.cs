using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransportManagementSystem.Models
{
    public class Vehicle
    {
        [Key]
        public int ViclId { get; set; }

        [Required]
        [StringLength(50)]
        public string ViclNum { get; set; } = default!;

        [Required]
        [StringLength(50)]

        public string ViclModel { get; set; } = default!;

        [Required]
        public int Capacity { get; set; }

        [Required]
        public string Status { get; set; } = "Available";

       // public Trip Trip { get; set; }


    }
}