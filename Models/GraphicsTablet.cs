namespace UczelnianaWypozyczalnia.Models
{
    public class GraphicsTablet : Equipment
    {
        public string WorkspaceArea { get; set; }
        public bool HasBuiltInScreen { get; set; }

        public GraphicsTablet(string name, string workspaceArea, bool hasBuiltInScreen) : base(name)
        {
            WorkspaceArea = workspaceArea;
            HasBuiltInScreen = hasBuiltInScreen;
        }
    }
}