using System.ComponentModel.DataAnnotations;

public class BaseEntity
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
}