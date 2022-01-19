using System.Threading.Tasks;
using Application.Interfaces;
using Shared;

namespace Application.Common
{
    public abstract class DTCBase<TENT, TDTO>
        where TENT : BaseEntity
        where TDTO : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        public DTCBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected virtual void ValidateAndThrow(TDTO dto)
        {
        }

        protected abstract TDTO MapToDTO(TENT ent);
        protected abstract TENT MapFromDTO(TDTO dto);
    }
}