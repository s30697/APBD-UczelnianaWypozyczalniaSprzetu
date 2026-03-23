namespace UczelnianaWypozyczalnia.Models;

public class Employee : User
{
    public override int MaxRentals => 5;
    
    public Employee(string firstName, string lastName, string email) : base(firstName, lastName, email) {}
}