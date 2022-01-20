using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain;

namespace Application
{
    public class SessionDTC : DTCBase<Session, SessionDTO>
    {
        public SessionDTC(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task CreateRangeAsync(List<SessionDTO> dtos)
        {
            foreach (SessionDTO dto in dtos)
            {
                Session ent = MapFromDTO(dto);
                await _unitOfWork.Sessions.CreateAsync(ent);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<SessionDTO>> ListByScheduleIdAsync(int scheduleId)
        {
            List<Session> efos = await _unitOfWork.Sessions.ListAsync(x => x.ScheduleId == scheduleId);
            return efos.Select(MapToDTO).ToList();
        }

        protected override Session MapFromDTO(SessionDTO dto)
        {
            throw new System.NotImplementedException();
        }

        protected override SessionDTO MapToDTO(Session ent)
        {
            throw new System.NotImplementedException();
        }
    }
}