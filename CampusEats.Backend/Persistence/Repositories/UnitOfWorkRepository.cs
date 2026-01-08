using CampusEats.Features.Menu.Interfaces;
using CampusEats.Persistence.Context;

namespace CampusEats.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CampusEatsDbContext _dbContext;

        public UnitOfWork(CampusEatsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}