using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Common
{
    public abstract class BaseAppService<TRq, TRs>
        where TRq : BaseRequest
        where TRs : BaseResponse
    {
        protected readonly IUnitOfWork _unitOfWork;
        public BaseAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public abstract Task<TRs> RunAsync(TRq rq);
    }
}