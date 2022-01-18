using Application.Interfaces;
using Domain;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public EFUnitOfWork(DataContext context)
        {
            _context = context;

            Schedules = new EFRepository<Schedule>(_context);
            Sessions = new EFRepository<Session>(_context);
            Branches = new EFRepository<Branch>(_context);
            Classes = new EFRepository<Class>(_context);
            Trainers = new EFRepository<Trainer>(_context);
        }

        public IRepository<Schedule> Schedules { get; private set; }
        public IRepository<Session> Sessions { get; private set; }
        public IRepository<Branch> Branches { get; private set; }
        public IRepository<Class> Classes { get; private set; }
        public IRepository<Trainer> Trainers { get; private set; }
        public IRepository<Registration> Registrations { get; private set; }

        public IDatabaseTransaction BeginTransaction()
        {
            return new EFDatabaseTransaction(_context.Database.BeginTransaction());
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
