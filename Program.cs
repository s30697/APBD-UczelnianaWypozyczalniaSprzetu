using UczelnianaWypozyczalnia.Models;
using UczelnianaWypozyczalnia.UI;

RentalService service = new RentalService();
UserUi userUi = new UserUi(service);
EquipmentUi equipmentUi = new EquipmentUi(service);
RentalUi rentalUi = new RentalUi(service);
ReportUi reportUi = new ReportUi(service);

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
    Console.WriteLine("11. Uruchom scenariusz demonstracyjny");
    Console.WriteLine("0. Zakończ program");
    Console.Write("Wybierz opcję: ");
    
    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                equipmentUi.ShowEquipment(onlyAvailable: false);
                break;
            case "2":
                equipmentUi.ShowEquipment(onlyAvailable: true);
                break;
            case "3":
                userUi.AddNewUser(); 
                break;
            case "4":
                equipmentUi.AddNewEquipment(); 
                break;
            case "5":
                rentalUi.RentEquipment();
                break;
            case "6":
                rentalUi.ReturnEquipment();
                break;
            case "7":
                equipmentUi.MarkEquipmentUnavailable();
                break;
            case "8":
                rentalUi.ShowUserActiveRentals();
                break;
            case "9":
                rentalUi.ShowOverdueRentals();
                break;
            case "10":
                reportUi.GenerateSummaryReport();
                break;
            case "11":
                RunDemo(service, reportUi);
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

void RunDemo(RentalService s, ReportUi rUi)
{
    Console.WriteLine("\nURUCHAMIANIE SCENARIUSZA DEMONSTRACYJNEGO");

    var demoStudent = new Student("Marek", "Demo", "marek@demo.pl");
    var demoEmployee = new Employee("Ewa", "Test", "ewa@test.pl");
    s.RegisterUser(demoStudent);
    s.RegisterUser(demoEmployee);

    var demoLaptop = new Laptop("Demo Laptop", "M1 Chip", 16);
    var demoCamera = new Camera("Demo Camera", "4K Professional", 50);
    s.RegisterEquipment(demoLaptop);
    s.RegisterEquipment(demoCamera);
    Console.WriteLine("-> Dodano użytkowników i sprzęt demonstracyjny.");

    var rental = s.RentEquipment(demoStudent, demoLaptop, 5);
    Console.WriteLine($"-> Pomyślnie wypożyczono {demoLaptop.Name} użytkownikowi {demoStudent.FirstName}.");

    Console.WriteLine("-> Próba wypożyczenia tego samego laptopa przez inną osobę.");
    try {
        s.RentEquipment(demoEmployee, demoLaptop, 2);
    } catch (Exception ex) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"-> Przechwycono błąd: {ex.Message}");
        Console.ResetColor();
    }

    s.ReturnEquipment(rental, DateTime.Now);
    Console.WriteLine($"-> Zwrot sprzętu {demoLaptop.Name} w terminie. Kara: {rental.Penalty} PLN.");
    var rentalLate = s.RentEquipment(demoEmployee, demoCamera, 1);
    
    DateTime lateDate = rentalLate.PlannedReturnDate.AddDays(10);
    s.ReturnEquipment(rentalLate, lateDate);
    
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"-> Zwrot {demoCamera.Name} po terminie (10 dni). Kara: {rentalLate.Penalty} PLN.");
    Console.ResetColor();

    Console.WriteLine("\n[RAPORT KOŃCOWY SCENARIUSZA]");
    rUi.GenerateSummaryReport();
    
    Console.WriteLine("KONIEC SCENARIUSZA");
}



