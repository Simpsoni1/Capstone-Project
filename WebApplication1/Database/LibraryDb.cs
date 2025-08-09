using Microsoft.EntityFrameworkCore;

class LibraryDb : DbContext
{
    public LibraryDb(DbContextOptions<LibraryDb> options) : base(options) { }

    public DbSet<Books> LibraryBooks => Set<Books>();
    public DbSet<Users> LibraryUsers => Set<Users>();
    public DbSet<Loans> LibraryLoans => Set<Loans>();
}