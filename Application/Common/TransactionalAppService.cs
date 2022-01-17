using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Common
{
    public abstract class TransactionalAppService<TRq, TRs> : BaseAppService<TRq, TRs>
        where TRq : BaseRequest
        where TRs : BaseResponse
    {
        protected TransactionalAppService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected sealed override async Task<TRs> RunAsync(TRq rq)
        {
            using (IDatabaseTransaction transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    TRs rs = await DoTransactionalRunAsync(rq);
                    transaction.Commit();

                    return rs;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        protected abstract Task<TRs> DoTransactionalRunAsync(TRq rq);
    }
}