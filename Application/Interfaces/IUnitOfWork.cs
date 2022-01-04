using System.Threading.Tasks;
using Domain;
using Shared;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        IDatabaseTransaction BeginTransaction();
        Task<int> SaveChangesAsync();
        IRepository<Schedule> Schedules { get; }
        IRepository<Session> Sessions { get; }
        IRepository<Branch> Branches { get; }
        IRepository<Class> Classes { get; }
        IRepository<Trainer> Trainers { get; }
    }
}