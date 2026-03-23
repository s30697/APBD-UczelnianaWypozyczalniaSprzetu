namespace UczelnianaWypozyczalnia.Models;

public class Projector : Equipment
{
    public string Resolution {get; set; }
    public int LampHours { get; set; }

    public Projector(string name, string resolution, int lampHours) : base(name)
    {
        Resolution = resolution;
        LampHours = lampHours;
    }
}