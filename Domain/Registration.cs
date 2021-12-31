using Shared;

namespace Domain
{
    public class Registration : BaseEntity
    {
        public Registration(int memberId, int sessionId)
        {
            MemberId = memberId;
            SessionId = sessionId;
        }

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