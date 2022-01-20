using System.Collections.Generic;
using Domain;

namespace Application.Interfaces
{
    public interface ISessionsGenerator
    {
        List<SessionDTO> Generate(ScheduleDTO schedule);
    }
}