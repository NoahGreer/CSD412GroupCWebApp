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
        [Display(Name = "Select Category")]
        public long[] SelectedCategoryIds { get; set; } = new long[0];
        public List<Category> CategoryOptions { get; set; }

        // This method is used to populate the options of the DropdownList
        public IEnumerable<SelectListItem> GetCategoryOptionsAsSelectListItems()
        {
            // Display the "Name" of the Category in DropDown 
            // Use "Id" for the selected value
            return new SelectList(CategoryOptions, "Id", "Name");
        }
    }
}
