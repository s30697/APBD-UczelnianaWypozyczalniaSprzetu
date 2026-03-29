using UczelnianaWypozyczalnia.Models;

namespace UczelnianaWypozyczalnia.UI;

public class RentalUI
{
    private readonly RentalService _service;

    public RentalUI(RentalService service)
    {
        _service = service;
    }
    
    public void RentEquipment()
    {
        Console.WriteLine("\n WYPOŻYCZENIE SPRZĘTU ");
        
        var users = _service.GetAllUsers();
        if (users.Count == 0)
        {
            Console.WriteLine("Brak użytkowników w systemie. Najpierw dodaj użytkownika.");
            return;
        }

        Console.WriteLine("Wybierz użytkownika (wpisz numer):");
        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {users[i].FirstName} {users[i].LastName} ({users[i].GetType().Name})");
        }
        
        if (!int.TryParse(Console.ReadLine(), out int userIndex) || userIndex < 1 || userIndex > users.Count)
        {
            Console.WriteLine("Błąd: Nieprawidłowy wybór użytkownika.");
            return;
        }
        User selectedUser = users[userIndex - 1];

        var availableEq = _service.GetAvailableEquipment();
        if (availableEq.Count == 0)
        {
            Console.WriteLine("Brak dostępnego sprzętu w magazynie.");
            return;
        }

        Console.WriteLine("\nWybierz sprzęt do wypożyczenia (wpisz numer):");
        for (int i = 0; i < availableEq.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {availableEq[i].Name} ({availableEq[i].GetType().Name})");
        }

        if (!int.TryParse(Console.ReadLine(), out int eqIndex) || eqIndex < 1 || eqIndex > availableEq.Count)
        {
            Console.WriteLine("Błąd: Nieprawidłowy wybór sprzętu.");
            return;
        }
        Equipment selectedEq = availableEq[eqIndex - 1];

        Console.Write("\nNa ile dni chcesz wypożyczyć sprzęt? ");
        if (!int.TryParse(Console.ReadLine(), out int days) || days <= 0)
        {
            Console.WriteLine("Błąd: Liczba dni musi być większa od zera.");
            return;
        }

        var rental = _service.RentEquipment(selectedUser, selectedEq, days);
        Console.WriteLine($"Sukces! Wypożyczono. Planowana data zwrotu: {rental.PlannedReturnDate.ToShortDateString()}");
    }

    public void ReturnEquipment()
    {
        Console.WriteLine("\n ZWROT SPRZĘTU ");
        
        var activeRentals = _service.GetAllActiveRentals();
        if (activeRentals.Count == 0)
        {
            Console.WriteLine("Brak aktywnych wypożyczeń w systemie.");
            return;
        }

        Console.WriteLine("Wybierz wypożyczenie do zwrotu (wpisz numer):");
        for (int i = 0; i < activeRentals.Count; i++)
        {
            var r = activeRentals[i];
            Console.WriteLine($"{i + 1}. Użytkownik: {r.User.FirstName} {r.User.LastName} | Sprzęt: {r.Equipment.Name} | Termin: {r.PlannedReturnDate.ToShortDateString()}");
        }

        if (!int.TryParse(Console.ReadLine(), out int rentalIndex) || rentalIndex < 1 || rentalIndex > activeRentals.Count)
        {
            Console.WriteLine("Błąd: Nieprawidłowy wybór.");
            return;
        }
        Rental selectedRental = activeRentals[rentalIndex - 1];

        Console.Write("\nIle dni po terminie następuje zwrot? (Wpisz 0, jeśli zwrot jest w terminie): ");
        if (!int.TryParse(Console.ReadLine(), out int daysLate) || daysLate < 0)
        {
            Console.WriteLine("Błąd: Liczba dni nie może być ujemna. Anulowano zwrot.");
            return;
        }

        DateTime returnDate = daysLate == 0 ? DateTime.Now : selectedRental.PlannedReturnDate.AddDays(daysLate);

        _service.ReturnEquipment(selectedRental, returnDate);
        
        Console.WriteLine($"Sukces! Sprzęt {selectedRental.Equipment.Name} został zwrócony.");
        if (selectedRental.Penalty > 0)
        {
            Console.WriteLine($"Uwaga! Z powodu {daysLate} dni opóźnienia, naliczono karę: {selectedRental.Penalty} PLN.");
        }
    }
    
    public void ShowUserActiveRentals()
    {
        Console.WriteLine("\n AKTYWNE WYPOŻYCZENIA UŻYTKOWNIKA ");
        var users = _service.GetAllUsers();
        
        if (users.Count == 0)
        {
            Console.WriteLine("Brak użytkowników w systemie.");
            return;
        }

        Console.WriteLine("Wybierz użytkownika (wpisz numer):");
        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {users[i].FirstName} {users[i].LastName}");
        }

        if (!int.TryParse(Console.ReadLine(), out int userIndex) || userIndex < 1 || userIndex > users.Count)
        {
            Console.WriteLine("Błąd: Nieprawidłowy wybór.");
            return;
        }

        User selectedUser = users[userIndex - 1];
        
        var userRentals = _service.GetActiveRentalsForUser(selectedUser);

        if (userRentals.Count == 0)
        {
            Console.WriteLine($"{selectedUser.FirstName} {selectedUser.LastName} nie ma obecnie żadnych aktywnych wypożyczeń.");
            return;
        }

        Console.WriteLine($"\nWypożyczenia użytkownika {selectedUser.FirstName} {selectedUser.LastName}:");
        foreach (var r in userRentals)
        {
            Console.WriteLine($"- {r.Equipment.Name} (Planowany zwrot: {r.PlannedReturnDate.ToShortDateString()})");
        }
    }

    public void ShowOverdueRentals()
    {
        Console.WriteLine("\nPRZETERMINOWANE WYPOŻYCZENIA ");
        
        var overdueRentals = _service.GetOverdueRentals();

        if (overdueRentals.Count == 0)
        {
            Console.WriteLine("Wspaniale! Brak przeterminowanych wypożyczeń.");
            return;
        }

        foreach (var r in overdueRentals)
        {
            int daysLate = (DateTime.Now.Date - r.PlannedReturnDate.Date).Days;
            Console.WriteLine($"- Użytkownik: {r.User.FirstName} {r.User.LastName} | Sprzęt: {r.Equipment.Name} | Opóźnienie: {daysLate} dni!");
        }
    }
}