using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDb>(opt => opt.UseInMemoryDatabase("Library"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/books", async (LibraryDb db) =>
    await db.LibraryBooks.ToListAsync());

app.MapGet("/books/{id}", async (int id, LibraryDb db) =>
    await db.LibraryBooks.FindAsync(id)
        is Books books
            ? Results.Ok(books)
            : Results.NotFound());

app.MapPost("/books", async (BooksDTO books, LibraryDb db) =>
{
    var booksToAdd = new Books
    {
        Name = books.Name,
        Author = books.Author
    };

    var updatedBooks = db.LibraryBooks.Add(booksToAdd);
    await db.SaveChangesAsync();

    return Results.Created($"/books/{updatedBooks.Entity.Id}", updatedBooks.Entity);
});

app.MapPut("/books/{id}", async (int id, BooksDTO inputBook, LibraryDb db) =>
{
    var book = await db.LibraryBooks.FindAsync(id);

    if (book is null) return Results.NotFound();

    book.Name = inputBook.Name;
    book.Author = inputBook.Author;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/books/{id}", async (int id, LibraryDb db) =>
{
    if (await db.LibraryBooks.FindAsync(id) is Books book)
    {
        db.LibraryBooks.Remove(book);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.MapGet("/users", async (LibraryDb db) =>
    await db.LibraryUsers.ToListAsync());

app.MapPost("/users", async (UserDTO users, LibraryDb db) =>
{
    var usersToAdd = new Users
    {
        Name = users.Name,
        Age = users.Age
    };

    var updatedUsers = db.LibraryUsers.Add(usersToAdd);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{updatedUsers.Entity.Id}", updatedUsers.Entity);
});

app.MapGet("/loans", async (LibraryDb db) =>
    await db.LibraryLoans.ToListAsync());

app.MapPost("/loans/checkout", async (LoansDTO loans, LibraryDb db) =>
{
    var loansToAdd = new Loans
    {
        BookId = loans.BookId,
        UserId = loans.UserId
    };

    var updatedLoans = db.LibraryLoans.Add(loansToAdd);
    await db.SaveChangesAsync();

    return Results.Created($"/loans/checkout/{updatedLoans.Entity.Id}", updatedLoans.Entity);
});

app.MapPost("/loans/returns", async (Loans loans, LibraryDb db) =>
{
    db.LibraryLoans.Add(loans);
    await db.SaveChangesAsync();

    return Results.Created($"/loans/returns/{loans.Id}", loans);
});

app.Run();