using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TheaterCashRegister.DAL.Data;
using TheaterCashRegister.DAL.Models;
using TheaterCashRegister.DAL.Repository.IRepository;

namespace TheaterCashRegister.DAL.Repository;

public class PerformanceRepository : Repository<Performance>, IPerformanceRepository
{
    private ApplicationDbContext _dbContext;

    public PerformanceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Performance obj)
    {
        _dbContext.Update(obj);
    }

    public Performance Get(Expression<Func<Performance, bool>> filter)
    {
        return _dbContext.Performance
            .Include(p => p.Tickets)
            .ThenInclude(t => t.Booking)
            .Where(filter)
            .FirstOrDefault();
    }
}