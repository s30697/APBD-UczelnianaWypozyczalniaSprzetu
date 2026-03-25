namespace UczelnianaWypozyczalnia.Models;

public class RentalService
{
    private readonly List<Rental> _rentals = new List<Rental>();

    private const decimal PenaltyPerDay = 5.0m;

    public Rental RentEquipment(User user, Equipment equipment, int days)
    {
        if (!equipment.IsAvailable)
        {
            throw new InvalidOperationException($"Sprzęt {equipment.Name} jest obecnie niedostępny.");
        }
        
        int activeRentalsCount = _rentals.Count(r => r.User.Id == user.Id && r.ActualReturnDate == null);
        
        if (activeRentalsCount >= user.MaxRentals)
        {
            throw new InvalidOperationException($"Użytkownik {user.FirstName} {user.LastName} osiągnął swój limit wypożyczeń ({user.MaxRentals}).");
        }
        
        Rental newRental = new Rental(user, equipment, days);
        
        equipment.MarkAsUnavailable();
        
        _rentals.Add(newRental);
        
        return newRental;
    }

    public void ReturnEquipment(Rental rental, DateTime returnDate)
    {
        if (rental.ActualReturnDate != null)
        {
            throw new InvalidOperationException("To wypożyczenie zostało już zakończone.");
        }
        
        rental.ReturnEquipment(returnDate);

        if (rental.IsOverdue)
        {
            int daysLate = (returnDate - rental.PlannedReturnDate).Days;

            if (daysLate > 0)
            {
                decimal totalPenalty = daysLate * PenaltyPerDay;
                rental.SetPenalty(totalPenalty);
            }
        }
    }
}