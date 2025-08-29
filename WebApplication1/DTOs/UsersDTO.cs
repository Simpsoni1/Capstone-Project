public class UserDTO
{
    public string? Name { get; set; }
    public int Age { get; set; }

    public UserDTO() { }
    public UserDTO(Users user) =>
    (Name, Age) = (user.Name, user.Age);
}