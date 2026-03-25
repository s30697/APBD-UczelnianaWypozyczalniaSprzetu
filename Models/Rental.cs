namespace UczelnianaWypozyczalnia.Models;

public class Rental
{
    public Guid Id { get; private set; }
    public User User { get; private set; }
    public Equipment Equipment { get; private set; }
    public DateTime RentalDate { get; private set; }
    public DateTime PlannedReturnDate { get; private set; }
    public DateTime? ActualReturnDate { get; private set; }
    public decimal Penalty { get; private set; }

    public Rental(User user, Equipment equipment, int rentalDays)
    {
        Id = Guid.NewGuid();
        User = user;
        Equipment = equipment;
        RentalDate = DateTime.Now;
        PlannedReturnDate = RentalDate.AddDays(rentalDays);
        ActualReturnDate = null;
        Penalty = 0;
    }

    public void ReturnEquipment(DateTime returnDate)
    {
        ActualReturnDate = returnDate;
        Equipment.MarkAsAvailable();
    }
    
    public void SetPenalty(decimal amount)
    {
        Penalty = amount;
    }
    
    public bool IsOverdue => ActualReturnDate == null 
        ? DateTime.Now > PlannedReturnDate 
        : ActualReturnDate > PlannedReturnDate;
}