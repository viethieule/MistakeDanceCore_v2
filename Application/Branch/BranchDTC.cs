using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain;

namespace Application
{
    public class BranchDTC : DTCBase<Branch, BranchDTO>
    {
        public BranchDTC(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task CreateAsync(BranchDTO dto)
        {
            ValidateAndThrow(dto);
            Branch ent = MapFromDTO(dto);

            await _unitOfWork.Branches.CreateAsync(ent);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = ent.Id;
        }

        protected override Branch MapFromDTO(BranchDTO dto)
        {
            throw new System.NotImplementedException();
        }

        protected override BranchDTO MapToDTO(Branch ent)
        {
            throw new System.NotImplementedException();
        }
    }
}