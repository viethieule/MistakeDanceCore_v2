using Shared;

namespace Domain
{
    public class Registration : BaseEntity
    {
        public int MemberId { get; private set; }
        public int SessionId { get; private set; }
        public RegistrationStatus Status { get; private set; }
    }

    public enum RegistrationStatus
    {
        Registered,
        Attended
    }
}