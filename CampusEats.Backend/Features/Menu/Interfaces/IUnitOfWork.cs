namespace CampusEats.Features.Menu.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}