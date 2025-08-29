
public class BooksDTO
{
    public string? Name { get; set; }
    public string? Author { get; set; }

    public BooksDTO() { }
    public BooksDTO(Books book) =>
    (Name, Author) = (book.Name, book.Author);
}