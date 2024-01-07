using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class BookCategory
    {
        public Book book;

        [Display(Name = "Category Name")]
        public string? CategoryName { get; set; }

        public BookCategory(Book book, string? categoryName)
        {
            this.book = book;
            CategoryName = categoryName;
        }
    }
}
