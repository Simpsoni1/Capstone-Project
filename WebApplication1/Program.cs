var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/books", () => "books");

app.MapGet("/", () => "Hello World!");

//app.MapGet("/books/{id}");

app.MapPost("/books");

app.Run();