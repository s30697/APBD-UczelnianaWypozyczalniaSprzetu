using UczelnianaWypozyczalnia.Models;

namespace UczelnianaWypozyczalnia;

public class MenuOperations
{
    private readonly RentalService _service;

    public MenuOperations(RentalService service)
    {
        _service = service;
    }

    public void AddNewUserInteractive()
    {
        Console.WriteLine("\n DODAWANIE NOWEGO UŻYTKOWNIKA ");
        
        Console.Write("Podaj imię: ");
        string firstName = Console.ReadLine() ?? ""; 
        
        Console.Write("Podaj nazwisko: ");
        string lastName = Console.ReadLine() ?? "";
        
        Console.Write("Podaj email: ");
        string email = Console.ReadLine() ?? "";

        Console.WriteLine("Wybierz typ użytkownika: \n1 - Student \n2 - Pracownik");
        Console.Write("Wybór: ");
        string typeChoice = Console.ReadLine() ?? "";

        if (typeChoice == "1")
        {
            _service.RegisterUser(new Student(firstName, lastName, email));
            Console.WriteLine("Sukces! Student został pomyślnie dodany do systemu.");
        }
        else if (typeChoice == "2")
        {
            _service.RegisterUser(new Employee(firstName, lastName, email));
            Console.WriteLine("Sukces! Pracownik został pomyślnie dodany do systemu.");
        }
        else
        {
            Console.WriteLine("Błąd: Nieznany typ użytkownika. Operacja anulowana.");
        }
    }

    public void AddNewEquipmentInteractive()
    {
        Console.WriteLine("\n DODAWANIE NOWEGO SPRZĘTU ");
        Console.WriteLine("Wybierz typ sprzętu:");
        Console.WriteLine("1 - Laptop");
        Console.WriteLine("2 - Projektor");
        Console.WriteLine("3 - Aparat fotograficzny");
        Console.WriteLine("4 - Tablet Graficzny");
        Console.Write("Wybór: ");
        
        string typeChoice = Console.ReadLine() ?? "";

        Console.Write("Podaj nazwę/model sprzętu: ");
        string name = Console.ReadLine() ?? "";

        switch (typeChoice)
        {
            case "1":
                Console.Write("Podaj model procesora: ");
                string cpu = Console.ReadLine() ?? "";
                Console.Write("Podaj ilość pamięci RAM (GB): ");
                if (int.TryParse(Console.ReadLine(), out int ram))
                {
                    _service.RegisterEquipment(new Laptop(name, cpu, ram));
                    Console.WriteLine("Sukces! Laptop został dodany.");
                }
                else Console.WriteLine("Błąd: RAM musi być poprawną liczbą całkowitą.");
                break;

            case "2":
                Console.Write("Podaj rozdzielczość (np. 1080p, XGA): ");
                string res = Console.ReadLine() ?? "";
                Console.Write("Podaj jasność (lumeny): ");
                if (int.TryParse(Console.ReadLine(), out int lumens))
                {
                    _service.RegisterEquipment(new Projector(name, res, lumens));
                    Console.WriteLine("Sukces! Projektor został dodany.");
                }
                else Console.WriteLine("Błąd: Jasność musi być poprawną liczbą całkowitą.");
                break;
                
            case "3":
                Console.Write("Podaj rozdzielczość nagrywania (np. 4K): ");
                string videoRes = Console.ReadLine() ?? "";
                Console.Write("Podaj liczbę megapikseli: ");
                if (int.TryParse(Console.ReadLine(), out int mp))
                {
                    _service.RegisterEquipment(new Camera(name, videoRes, mp));
                    Console.WriteLine("Sukces! Aparat został dodany.");
                }
                else Console.WriteLine("Błąd: Megapiksele muszą być poprawną liczbą całkowitą.");
                break;

            case "4":
                Console.Write("Podaj obszar roboczy (np. A4): ");
                string workspace = Console.ReadLine() ?? "";
                Console.Write("Czy posiada wbudowany ekran? (T/N): ");
                string hasScreenStr = Console.ReadLine()?.ToUpper() ?? "";
                bool hasScreen = hasScreenStr == "T"; 
                
                _service.RegisterEquipment(new GraphicsTablet(name, workspace, hasScreen));
                Console.WriteLine("Sukces! Tablet graficzny został dodany.");
                break;

            default:
                Console.WriteLine("Błąd: Nieznany typ sprzętu. Operacja anulowana.");
                break;
        }
    }
    
    public void ShowEquipment(bool onlyAvailable)
    {
        var equipmentList = onlyAvailable ? _service.GetAvailableEquipment() : _service.GetAllEquipment();
        
        Console.WriteLine(onlyAvailable ? "\nTylko dostępne narzędzia! " : "\nKatalog urządzeń pełny! ");

        if (equipmentList.Count == 0)
        {
            Console.WriteLine("Brak sprzętu w tej kategorii.");
            return;
        }

        foreach (var eq in equipmentList)
        {
            string status = eq.IsAvailable ? "Dostępny!" : "Wypożyczony!";
            Console.WriteLine($"-  {eq.Name}  [{status}]");
        }
    }
    
    public void RentEquipmentInteractive()
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
        User selectedUser = users[userIndex - 1]; // -1 bo listy indeksujemy od 0

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

    public void ReturnEquipmentInteractive()
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
}