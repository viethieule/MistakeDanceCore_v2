using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain;

namespace Application
{
    public class ClassDTC : DTCBase<Class, ClassDTO>
    {
        public ClassDTC(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task CreateAsync(ClassDTO dto)
        {
            ValidateAndThrow(dto);
            Class ent = MapFromDTO(dto);
            
            await _unitOfWork.Classes.CreateAsync(ent);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = ent.Id;
        }

        protected override Class MapFromDTO(ClassDTO dto)
        {
            throw new System.NotImplementedException();
        }

        protected override ClassDTO MapToDTO(Class ent)
        {
            throw new System.NotImplementedException();
        }
    }
}