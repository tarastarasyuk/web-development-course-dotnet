using System.Linq.Expressions;
using TheaterCashRegister.DAL.Models;

namespace TheaterCashRegister.DAL.Repository.IRepository;

public interface IPerformanceRepository : IRepository<Performance>
{
    void Update(Performance obj);
    Performance Get(Expression<Func<Performance, bool>> filter);
}