using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412GroupCWebApp.Models
{
    public class Blog
    {
        public long Id { get; set; }
        public string Authors { get; set; }
        public string Title { get; set; }
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        [Display(Name = "Create Date")]
        public string CreateDate { get; set; }
    }
}
