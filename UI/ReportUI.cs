using UczelnianaWypozyczalnia.Models;

namespace UczelnianaWypozyczalnia.UI;

public class ReportUI
{
    
    private readonly RentalService _service;

    public ReportUI(RentalService service)
    {
        _service = service;
    }
    
    public void GenerateSummaryReport()
    {
        Console.WriteLine("\nRAPORT PODSUMOWUJĄCY STAN WYPOŻYCZALNI ");
        
        int totalUsers = _service.GetAllUsers().Count;
        int totalEquipment = _service.GetAllEquipment().Count;
        int availableEquipment = _service.GetAvailableEquipment().Count;
        int activeRentals = _service.GetAllActiveRentals().Count;
        
        int overdueRentals = _service.GetOverdueRentals().Count; 
        
        int unavailableOrDamaged = totalEquipment - availableEquipment - activeRentals;

        Console.WriteLine($"Zarejestrowani użytkownicy:     {totalUsers}");
        Console.WriteLine($"Całkowity sprzęt w systemie:    {totalEquipment}");
        Console.WriteLine($"Sprzęt dostępny do wydania:     {availableEquipment}");
        Console.WriteLine($"Sprzęt w naprawie:  {unavailableOrDamaged}");
        Console.WriteLine($"Aktywne wypożyczenia:           {activeRentals}");
        Console.WriteLine($"W tym przeterminowane:          {overdueRentals}");
    }
}