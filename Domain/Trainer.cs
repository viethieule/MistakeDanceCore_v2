using Shared;

namespace Domain
{
    public class Trainer : BaseEntity
    {
        public Trainer(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}