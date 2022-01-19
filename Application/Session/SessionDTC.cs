using System.Collections.Generic;
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