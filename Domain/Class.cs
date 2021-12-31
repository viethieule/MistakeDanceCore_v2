using Shared;

namespace Domain
{
    public class Class : BaseEntity
    {
        public Class(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}