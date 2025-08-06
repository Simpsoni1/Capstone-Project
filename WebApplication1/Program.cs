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
    await db.Library.ToListAsync());

app.MapGet("/books/{id}", async (int id, LibraryDb db) =>
    await db.Library.FindAsync(id)
        is Books books
            ? Results.Ok(books)
            : Results.NotFound());

app.MapPost("/books", async (Books books, LibraryDb db) =>
{
    db.Library.Add(books);
    await db.SaveChangesAsync();

    return Results.Created($"/books/{books.Id}", books);
});

app.MapPut("/books/{id}", async (int id, Books inputBook, LibraryDb db) =>
{
    var book = await db.Library.FindAsync(id);

    if (book is null) return Results.NotFound();

    book.Name = inputBook.Name;
    book.Author = inputBook.Author;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/books/{id}", async (int id, LibraryDb db) =>
{
    if (await db.Library.FindAsync(id) is Books book)
    {
        db.Library.Remove(book);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

// app.MapGet("/users", () => "users");

// app.MapPost("/users", () => "");

// app.MapGet("/loans", () => "users");

// app.MapPost("/loans", () => "");

// app.MapPost("/returns", () => "");

app.Run();