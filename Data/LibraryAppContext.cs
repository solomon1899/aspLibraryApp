using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Models;

namespace Library.Data
{
    public class LibraryAppContext : DbContext
    {
        public LibraryAppContext (DbContextOptions<LibraryAppContext> options)
            : base(options)
        {
        }

        public DbSet<LibraryApp.Models.Book> Book { get; set; } = default!;
        public DbSet<LibraryApp.Models.Category> Category { get; set; } = default!;
        public DbSet<LibraryApp.Models.Borrow> Borrow { get; set; } = default!;
    }
}
