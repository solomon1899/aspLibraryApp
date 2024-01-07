using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        [Display(Name = "Number of copies")]
        public int NumberOfCopies { get; set; }
        public string? Description { get; set; }
        public int Category { get; set; }
        [Display(Name = "")]
        public string? ImageUrl { get; set; }
    }

}
