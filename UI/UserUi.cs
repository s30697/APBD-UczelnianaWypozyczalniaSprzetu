using UczelnianaWypozyczalnia.Models;

namespace UczelnianaWypozyczalnia.UI;

public class UserUi
{
    private readonly RentalService _service;

    public UserUi(RentalService service)
    {
        _service = service;
    }
    
    public void AddNewUser()
    {
        Console.WriteLine("\nDODAWANIE NOWEGO UŻYTKOWNIKA ");
        
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
}