using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412GroupCWebApp.Models
{
    public class ArticleViewModel
    {
        public Article Article { get; set; }
        public List<Category> Categories { get; set; }

        // This method is used to populate the options of the DropdownList
        public IEnumerable<SelectListItem> GetCategoriesAsSelectListItems()
        {
            // Display the "Name" of the Category in DropDown 
            // Use "Id" for the selected value
            return new SelectList(Categories, "Id", "Name");
        }
    }
}
