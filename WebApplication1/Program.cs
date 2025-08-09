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

app.MapPost("/books", async (Books books, LibraryDb db) =>
{
    db.LibraryBooks.Add(books);
    await db.SaveChangesAsync();

    return Results.Created($"/books/{books.Id}", books);
});

app.MapPut("/books/{id}", async (int id, Books inputBook, LibraryDb db) =>
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

app.MapPost("/users", async (Users users, LibraryDb db) =>
{
    db.LibraryUsers.Add(users);
    await db.SaveChangesAsync();

    return Results.Created($"/user/{users.Id}", users);
});

app.MapGet("/loans", async (LibraryDb db) =>
    await db.LibraryLoans.ToListAsync());

app.MapPost("/loans", async (Loans loans, LibraryDb db) =>
{
    db.LibraryLoans.Add(loans);
    await db.SaveChangesAsync();

    return Results.Created($"/loans/{loans.Id}", loans);
});

app.MapPost("/returns", async (Loans loans, LibraryDb db) =>
{
    db.LibraryLoans.Add(loans);
    await db.SaveChangesAsync();

    return Results.Created($"/returns/{loans.Id}", loans);
});

app.Run();