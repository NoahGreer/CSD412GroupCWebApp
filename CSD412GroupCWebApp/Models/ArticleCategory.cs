using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412GroupCWebApp.Models
{
    public class ArticleCategory
    {
        public long ArticleId { get; set; }
        public Article Article { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
