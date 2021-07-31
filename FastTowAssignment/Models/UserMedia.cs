using FastTowAssignment.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FastTowAssignment.Models
{
    public class UserMedia
    {
        [Key]
        public int MediaId { get; set; }
        public string UserId { get; set; }
        public string MediaFileName { get; set; }
        public string MediaFileType { get; set; }
        public string MediaUrl { get; set; }
        public DateTime DateTimeUploaded { get; set; }
        public virtual FastTowAssignmentUser User { get; set; }
    }
}
