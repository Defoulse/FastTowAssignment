using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FastTowAssignment.Areas.Identity.Data;

namespace FastTowAssignment.Models
{
    public class Order
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual FastTowAssignmentUser Client { get; set; }
        public string DriverId { get; set; }
        [ForeignKey("DriverId")]
        public virtual FastTowAssignmentUser Driver { get; set; }
        [Required]
        [Display(Name = "Car Departure City")]
        public long DepartureCityId { get; set; }
        [Required]
        [Display(Name = "Car Destination City")]
        public long DestinationCityId { get; set; }
        [ForeignKey("DepartureCityId")]
        public virtual City DepartureCity { get; set; }
        [ForeignKey("DestinationCityId")]
        public virtual City DestinationCity { get; set; }
        [Required]
        [Display(Name = "Price for Transpartation")]
        public int Price { get; set; }
        [Required]
        [Display(Name = "Car Model")]
        public string Car { get; set; }
        [Required]
        [Display(Name = "Current Status")]
        public long CurrentStatusId { get; set; }
        [ForeignKey("CurrentStatusId")]
        public virtual Status CurrentStatus { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Order Created")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
    }
}
