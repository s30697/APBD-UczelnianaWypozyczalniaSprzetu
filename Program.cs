using UczelnianaWypozyczalnia.Models;
using UczelnianaWypozyczalnia.UI;

RentalService service = new RentalService();
UserUI userUI = new UserUI(service);
EquipmentUI equipmentUI = new EquipmentUI(service);
RentalUI rentalUI = new RentalUI(service);
ReportUI reportUI = new ReportUI(service);

SeedData(service);

bool exit = false;
while (!exit)
{
    Console.WriteLine("\nUCZELNIANA WYPOŻYCZALNIA SPRZĘTU");
    Console.WriteLine("1. Wyświetl cały sprzęt");
    Console.WriteLine("2. Wyświetl tylko dostępny sprzęt");
    Console.WriteLine("3. Dodaj nowego użytkownika");
    Console.WriteLine("4. Dodaj nowy sprzęt");
    Console.WriteLine("5. Wypożycz sprzęt");
    Console.WriteLine("6. Zwróć sprzęt");
    Console.WriteLine("7. Zgłoś niedostępność sprzętu");
    Console.WriteLine("8. Wyświetl aktywne wypożyczenia użytkownika");
    Console.WriteLine("9. Wyświetl przeterminowane wypożyczenia");
    Console.WriteLine("10. Generuj raport podsumowujący");
    Console.WriteLine("0. Zakończ program");
    Console.Write("Wybierz opcję: ");
    
    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                equipmentUI.ShowEquipment(onlyAvailable: false);
                break;
            case "2":
                equipmentUI.ShowEquipment(onlyAvailable: true);
                break;
            case "3":
                userUI.AddNewUser(); 
                break;
            case "4":
                equipmentUI.AddNewEquipment(); 
                break;
            case "5":
                rentalUI.RentEquipment();
                break;
            case "6":
                rentalUI.ReturnEquipment();
                break;
            case "7":
                equipmentUI.MarkEquipmentUnavailable();
                break;
            case "8":
                rentalUI.ShowUserActiveRentals();
                break;
            case "9":
                rentalUI.ShowOverdueRentals();
                break;
            case "10":
                reportUI.GenerateSummaryReport();
                break;
            case "0":
                exit = true;
                break;
            default:
                Console.WriteLine("Nieznana opcja! Spróbuj ponownie!");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nBŁĄD: {ex.Message}");
        Console.ResetColor();
    }
}

void SeedData(RentalService s)
{
    s.RegisterUser(new Employee("Andrzej", "Barański", "andrzej.baranski@uczelnia.pl"));
    s.RegisterUser(new Employee("Magdalena", "Dziekańska", "magdalena.dziekanska@uczelnia.pl"));
    s.RegisterUser(new Employee("Tomasz", "Jeżycki", "tomasz.jezycki@uczelnia.pl"));
    s.RegisterUser(new Employee("Katarzyna", "Kalisińska", "katarzyna.kalisinska@uczelnia.pl"));
    
    s.RegisterUser(new Student("Stefan", "Pawłowski", "pawlowski.stefan@student.pl"));
    s.RegisterUser(new Student("Danuta", "Leśnicka", "lesnicka.danuta@student.pl"));
    s.RegisterUser(new Student("Ji-Woo", "Park", "jiwoo.park@student.pl")); 

    s.RegisterEquipment(new Laptop("Dell XPS 15", "Intel i7", 16));
    s.RegisterEquipment(new Laptop("Asus ROG Strix", "AMD Ryzen 9", 32));
    s.RegisterEquipment(new Laptop("ThinkPad T14", "Intel i5", 16));

    s.RegisterEquipment(new Projector("Epson EB-X41", "XGA", 3300));
    s.RegisterEquipment(new Projector("BenQ TH585", "1080p", 3500));

    s.RegisterEquipment(new Camera("Sony A7 III", "4K", 24));
    s.RegisterEquipment(new Camera("Canon EOS R5", "8K", 45));

    s.RegisterEquipment(new GraphicsTablet("Wacom Intuos Pro", "A4", false));
    s.RegisterEquipment(new GraphicsTablet("Huion Kamvas 22", "21.5 cala", true));
}