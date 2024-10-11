using Microsoft.EntityFrameworkCore;
using BooksApp.Data;

namespace BooksApp
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<BookData> Books { get; set; }
        public DbSet<CategoryData> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
