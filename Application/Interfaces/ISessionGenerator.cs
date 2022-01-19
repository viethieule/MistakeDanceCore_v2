using System.Collections.Generic;
using Domain;

namespace Application.Interfaces
{
    public interface ISessionGenerator
    {
        List<SessionDTO> Generate(ScheduleDTO schedule);
    }
}