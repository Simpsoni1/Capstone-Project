using Microsoft.EntityFrameworkCore;

class LibraryDb : DbContext
{
    public LibraryDb(DbContextOptions<LibraryDb> options) : base(options) { }

    public DbSet<Books> Library => Set<Books>();
}