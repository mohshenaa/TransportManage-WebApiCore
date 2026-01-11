using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransportManagementSystem.Models
{
    public class Driver
    {
        [Key]
        public int DriId { get; set; }

        [Required, StringLength(50)]
        public string DriName { get; set; } = default!;

        [Required,StringLength(100)]
        public string LicenseNum { get; set; } = default!;

        [Required,StringLength(50)]
        public string Contact { get; set; } = default!;

        public string ImageUrl { get; set; } = "";

        public bool IsAvailable { get; set; } = true;

        //public Trip Trip { get; set; }


    }
}