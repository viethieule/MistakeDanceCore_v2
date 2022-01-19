using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain;

namespace Application
{
    public class TrainerDTC : DTCBase<Trainer, TrainerDTO>
    {
        public TrainerDTC(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task CreateAsync(TrainerDTO dto)
        {
            ValidateAndThrow(dto);
            Trainer ent = MapFromDTO(dto);

            await _unitOfWork.Trainers.CreateAsync(ent);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = ent.Id;
        }

        protected override Trainer MapFromDTO(TrainerDTO dto)
        {
            throw new System.NotImplementedException();
        }

        protected override TrainerDTO MapToDTO(Trainer efo)
        {
            throw new System.NotImplementedException();
        }
    }
}