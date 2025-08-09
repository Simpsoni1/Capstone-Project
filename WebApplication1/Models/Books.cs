public class Books
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Author { get; set; }
    public bool IsLoaned { get; set; }


    //Create a book
    public Books(string Name, string Author)
    {
        Id = +1;
        this.Name = Name;
        this.Author = Author;
        IsLoaned = false;
    }

}