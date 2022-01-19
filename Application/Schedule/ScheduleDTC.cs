using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain;

namespace Application
{
    public class ScheduleDTC : DTCBase<Schedule, ScheduleDTO>
    {
        public ScheduleDTC(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task CreateAsync(ScheduleDTO dto)
        {
            ValidateAndThrow(dto);
            Schedule ent = MapFromDTO(dto);
            
            await _unitOfWork.Schedules.CreateAsync(ent);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = ent.Id;
        }

        protected override Schedule MapFromDTO(ScheduleDTO dto)
        {
            throw new System.NotImplementedException();
        }

        protected override ScheduleDTO MapToDTO(Schedule ent)
        {
            throw new System.NotImplementedException();
        }

        protected sealed override void ValidateAndThrow(ScheduleDTO dto)
        {
        }
    }
}