using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412GroupCWebApp.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Articles")]
        public virtual ICollection<ArticleCategory> ArticleCategories { get; set; }
    }
}
