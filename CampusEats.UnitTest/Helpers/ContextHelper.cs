using CampusEats.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.UnitTest.Helpers
{
    public static class ContextHelper
    {
        public static CampusEatsDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CampusEatsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new CampusEatsDbContext(options);
        }
    }
}