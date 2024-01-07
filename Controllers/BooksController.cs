using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using LibraryApp.Models;
using System.Security.Cryptography;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryAppContext _context;

        public BooksController(LibraryAppContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                var books = await _context.Book.ToListAsync();
                var categories = await _context.Category.ToListAsync();

                var bookByCategory = from book in books     // ViewModel for books with their category name
                                     join category in categories
                                     on book.Category equals category.Id
                                     where category.Id == id      // Requeted category
                                     select new BookCategory(book, category.CategoryName);
                return View(bookByCategory);
            }
            else
            {
                var books = await _context.Book.ToListAsync();
                var categories = await _context.Category.ToListAsync();

                var bookByCategory = from book in books
                                     join category in categories
                                     on book.Category equals category.Id
                                     select new BookCategory(book, category.CategoryName);
                return View(bookByCategory);
            }
            
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FirstOrDefaultAsync(m => m.Id == id);
            var categories = await _context.Category.ToListAsync();

            if (book == null)
            {
                return NotFound();
            }

            var categoryName = (from category in categories
                                where category.Id == book.Category
                                select category.CategoryName).FirstOrDefault();

            BookCategory bookCategory = new BookCategory(book, categoryName);

            return View(bookCategory);
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
