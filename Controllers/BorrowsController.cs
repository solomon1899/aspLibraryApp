using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Identity;
using LibraryApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace LibraryApp.Controllers
{
    public class BorrowsController : Controller
    {
        private readonly LibraryAppContext _context;
        private readonly UserManager<LibraryAppUser> _userManager;
        private readonly EmailSender _emailSender;
        private readonly PdfService _pdfService;


        public BorrowsController(LibraryAppContext context, UserManager<LibraryAppUser> userManager, EmailSender emailSender, PdfService pdfService)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _pdfService = pdfService;
        }

        // GET: Borrowings of logged user
        public async Task<IActionResult> Index(string? id)
        {
            List<Book> books = await _context.Book.ToListAsync();
            List<Borrow> borrows = await _context.Borrow.ToListAsync();

            var borrowsOfUser = borrows.Where(b => b.MemberId == id).ToList();  // list of borrowing transactions of user

            var result = from borrow in borrowsOfUser
                         join book in books on borrow.BookId equals book.Id
                         select new BookBorrow      // ViewModel for borrowings with the book name unstead of id
                         {
                             Title = book.Title,
                             DateBorrowed = borrow.DateBorrowed,
                             DueDate = borrow.DueDate
                         };
            return View(result);
        }

        // GET: Borrows/AddTransaction
        [Authorize]
        public IActionResult AddTransaction()
        {
            return View();
        }

        // POST: Borrows/AddTransaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddTransaction(int id, [Bind("DateBorrowed,DueDate")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = _userManager.GetUserId(User)!;
                borrow.MemberId = currentUserId;    // gets the id of the logged user
                borrow.BookId = id;     // The id of the book that the user chose
                _context.Add(borrow);
                await _context.SaveChangesAsync();


                string username = _userManager.GetUserName(this.User);
                var book = await _context.Book.FirstOrDefaultAsync(m => m.Id == id);

                //pdf settings
                //string htmlContent = @"<html><body><h1>Hello"+book.Title+"</h1></body></html>";
                string htmlContent = @"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Book Details</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    margin: 20px;
                }

                h1 {
                    color: #333;
                }

                .book-container {
                    display: flex;
                    align-items: center;
                    margin-top: 20px;
                }

                .book-details {
                    margin-left: 20px;
                }

                img {
                    max-width: 150px;
                    height: auto;
                }

                .due-date {
                    color: #777;
                }
            </style>
        </head>
        <body>
            <h1>" + book.Description + "</h1><div class='book-container'><img src='" + book.ImageUrl + "' alt='Book Cover'><div class='book-details'><h2>" + book.Title + "</h2><p class='due-date'> please return it on time or there will be consequences" + borrow.DueDate + "</p></div></div></body></html>";
                string outputPath = "wwwroot/pdf/" + book.Id + currentUserId + ".pdf";
                _pdfService.GeneratePdf(htmlContent, outputPath);
                //email settings
                string subject = "book borrow";
                string body = "you have borrowed " + book.Title + " till " + borrow.DueDate + ".";
                await _emailSender.SendEmailAsync(username, subject, body, outputPath);

                TempData["SuccessMessage"] = "The Book Was Added Successfully.";
                return RedirectToAction(nameof(Index), "Books");
            }
            return View(borrow);
        }

        private bool BorrowExists(int id)
        {
            return _context.Borrow.Any(e => e.LoanId == id);
        }
    }
}
