using UczelnianaWypozyczalnia.Models;

namespace UczelnianaWypozyczalnia.UI;

public class EquipmentUI
{
    
    private readonly RentalService _service;

    public EquipmentUI(RentalService service)
    {
        _service = service;
    }
    
    public void AddNewEquipment()
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

        var activeRentals = _service.GetAllActiveRentals();

        foreach (var eq in equipmentList)
        {
            string status;
            if (eq.IsAvailable)
            {
                status = "Dostępny!";
            }
            else
            {
                bool isRented = activeRentals.Any(r => r.Equipment.Id == eq.Id);
                status = isRented ? "Wypożyczony!" : "W naprawie!";
            }

            Console.WriteLine($"-  {eq.Name}  [{status}]");
        }
    }
    
    public void MarkEquipmentUnavailable()
    {
        Console.WriteLine("\n ZMIANA STATUSU SPRZĘTU NA NIEDOSTĘPNY ");
        var availableEq = _service.GetAvailableEquipment();
        
        if (availableEq.Count == 0)
        {
            Console.WriteLine("Brak dostępnego sprzętu, który można by oznaczyć jako uszkodzony lub w serwisie.");
            return;
        }

        Console.WriteLine("Wybierz sprzęt do wycofania (wpisz numer):");
        for (int i = 0; i < availableEq.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {availableEq[i].Name} ({availableEq[i].GetType().Name})");
        }

        if (!int.TryParse(Console.ReadLine(), out int eqIndex) || eqIndex < 1 || eqIndex > availableEq.Count)
        {
            Console.WriteLine("Błąd: Nieprawidłowy wybór.");
            return;
        }

        Equipment selectedEq = availableEq[eqIndex - 1];
        
        selectedEq.MarkAsUnavailable(); 
        
        Console.WriteLine($"Sukces! Sprzęt '{selectedEq.Name}' został oznaczony jako niedostępny.");
    }
}