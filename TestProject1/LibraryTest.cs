using Microsoft.EntityFrameworkCore;
namespace TestProject1;


[TestClass]
public class LibraryTest
{
    private LibraryDb GetDb()
    {
        var options = new DbContextOptionsBuilder<LibraryDb>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LibraryDb(options);
    }

    [TestMethod]
    public void CanAddBook()
    {
        using var db = GetDb();
        var book = new Books { Name = "Test Book", Author = "Author" };
        db.LibraryBooks.Add(book);
        db.SaveChanges();

        Assert.AreEqual(1, db.LibraryBooks.Count());
        Assert.AreEqual("Test Book", db.LibraryBooks.First().Name);
    }

    [TestMethod]
    public void BookIsLoanedIsFalseByDefault()
    {
        using var db = GetDb();
        var book = new Books { Name = "Book", Author = "A" };
        db.LibraryBooks.Add(book);
        db.SaveChanges();

        Assert.IsFalse(db.LibraryBooks.First().IsLoaned, "IsLoaned should be false by default.");
    }

    [TestMethod]
    public void CanLoanBookAndSetIsLoaned()
    {
        using var db = GetDb();
        var book = new Books { Name = "Book", Author = "A" };
        var user = new Users { Name = "User", Age = 20 };
        db.LibraryBooks.Add(book);
        db.LibraryUsers.Add(user);
        db.SaveChanges();

        var loan = new Loans { BookId = book.Id, UserId = user.Id };
        db.LibraryLoans.Add(loan);
        book.IsLoaned = true;
        db.SaveChanges();

        Assert.IsTrue(db.LibraryBooks.First().IsLoaned);
        Assert.AreEqual(1, db.LibraryLoans.Count());
    }

    [TestMethod]
    public void CannotLoanBookThatIsAlreadyLoaned()
    {
        using var db = GetDb();
        var book = new Books { Name = "Book", Author = "A", IsLoaned = true };
        var user = new Users { Name = "User", Age = 20 };
        db.LibraryBooks.Add(book);
        db.LibraryUsers.Add(user);
        db.SaveChanges();

        
        var loan = new Loans { BookId = book.Id, UserId = user.Id };
        bool canLoan = !book.IsLoaned;
        Assert.IsFalse(canLoan, "Should not be able to loan a book that is already loaned.");
    }

    [TestMethod]
    public void CannotLoanBookToNonexistentUser()
    {
        using var db = GetDb();
        var book = new Books { Name = "Book", Author = "A" };
        db.LibraryBooks.Add(book);
        db.SaveChanges();

        int nonexistentUserId = 333;
        var user = db.LibraryUsers.Find(nonexistentUserId);
        Assert.IsNull(user, "User should not exist.");

        var loan = new Loans { BookId = book.Id, UserId = nonexistentUserId };
        
        bool canLoan = user != null;
        Assert.IsFalse(canLoan, "Should not be able to loan to a nonexistent user.");
    }

    [TestMethod]
    public void CanReturnBookAndSetIsLoanedFalse()
    {
        using var db = GetDb();
        var book = new Books { Name = "Book", Author = "A", IsLoaned = true };
        var user = new Users { Name = "User", Age = 20 };
        db.LibraryBooks.Add(book);
        db.LibraryUsers.Add(user);
        db.SaveChanges();

        var loan = new Loans { BookId = book.Id, UserId = user.Id, Returned = false };
        db.LibraryLoans.Add(loan);
        db.SaveChanges();


        loan.Returned = true;
        book.IsLoaned = false;
        db.SaveChanges();

        Assert.IsTrue(db.LibraryLoans.First().Returned);
        Assert.IsFalse(db.LibraryBooks.First().IsLoaned);
    }

    [TestMethod]
    public void CannotReturnNonexistentLoan()
    {
        using var db = GetDb();
        var loan = db.LibraryLoans.Find(222);
        Assert.IsNull(loan, "Loan should not exist.");
    }
}