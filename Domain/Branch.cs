using Shared;

namespace Domain
{
    public class Branch : BaseEntity
    {
        public Branch(string name, string abbreviation, string address)
        {
            Name = name;
            Abbreviation = abbreviation;
            Address = address;
        }

        public string Name { get; private set; }
        public string Abbreviation { get; private set; }
        public string Address { get; private set; }
    }
}