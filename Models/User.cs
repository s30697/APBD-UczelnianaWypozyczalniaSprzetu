namespace UczelnianaWypozyczalnia.Models;

public abstract class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    public string Email { get; set; } 
    
    public abstract int MaxRentals { get; }
    protected User(string firstName, string lastName, string email)
        {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        }
}