using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Category Name")]
        public string? CategoryName { get; set; }
    }

}
