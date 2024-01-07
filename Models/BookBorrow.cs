using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class BookBorrow
    {
        public string? Title { get; set; }
        [Display(Name = "Date Borrowed")]
        public DateTime DateBorrowed { get; set; }
        [Display(Name = "Due Borrowed")]
        public DateTime DueDate { get; set; }
    }

}
