namespace UczelnianaWypozyczalnia.Models;

public class Camera : Equipment
{
    public string LensType { get; set; }
    public int Megapixels { get; set; }
    
    public Camera(string name, string lensType, int megapixels) : base(name)
    {
        LensType = lensType;
        Megapixels = megapixels;
    }
}