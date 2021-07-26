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
        public long DepartureCityId { get; set; }
        [Required]
        public long DestinationCityId { get; set; }
        [ForeignKey("DepartureCityId")]
        public virtual City DepartureCity { get; set; }
        [ForeignKey("DestinationCityId")]
        public virtual City DestinationCity { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Car { get; set; }
        [Required]
        public long CurrentStatusId { get; set; }
        [ForeignKey("CurrentStatusId")]
        public virtual Status CurrentStatus { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
    }
}
