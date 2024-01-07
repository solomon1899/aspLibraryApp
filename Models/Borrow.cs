using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Borrow
    {
        [Key]
        public int LoanId { get; set; }
        [Display(Name = "Book Id")]
        public int BookId { get; set; }
        [Display(Name = "Member Id")]
        public string? MemberId { get; set; }
        [Display(Name = "Date Borrowed")]
        public DateTime DateBorrowed { get; set; }
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

    }


}
