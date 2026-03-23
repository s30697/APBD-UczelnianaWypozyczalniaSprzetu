namespace UczelnianaWypozyczalnia.Models;

public class Student : User
{
    public override int MaxRentals => 2;

    public Student(string firstName, string lastName, string email) : base(firstName, lastName, email) {}
}