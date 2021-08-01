using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FastTowAssignment.Models
{
    public class Article
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
