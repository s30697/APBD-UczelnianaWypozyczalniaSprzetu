namespace UczelnianaWypozyczalnia.Models;

    public abstract class Equipment
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public bool IsAvailable { get; protected set; }

        protected Equipment(string name)
        {
            Id = Guid.NewGuid(); 
            Name = name;
            IsAvailable = true;
        }

        public void MarkAsUnavailable()
        {
            IsAvailable = false;
        }

        public void MarkAsAvailable()
        {
            IsAvailable = true;
        }
    }
