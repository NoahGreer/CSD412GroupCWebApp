using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412GroupCWebApp.Models
{
    public class Article
    {
        public long Id { get; set; }
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; } = false;
        [Display(Name = "URL Slug")]
        public string UrlSlug { get; set; }
        public virtual List<Category> Categories { get; set; }
        [Display(Name = "Date Posted")]
        public DateTime? DatePosted { get; set; }
    }
}
