var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/books", () => "books");

app.MapGet("/books/{id}", () => 1);

app.MapPost("/books", () => "");

app.MapPut("/books/{id}", () => "");

app.MapDelete("/books/{id}", () => "");

app.MapGet("/users", () => "users");

app.MapPost("/users", () => "");

app.MapGet("/loans", () => "users");

app.MapPost("/loans", () => "");

app.MapPost("/returns", () => "");

app.Run();