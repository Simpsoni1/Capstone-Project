
public class LoansDTO
{
    public int BookId { get; set; }
    public int UserId { get; set; }

    public LoansDTO() { }
    public LoansDTO(Loans loan) =>
    (BookId, UserId) = (loan.BookId, loan.UserId);
}