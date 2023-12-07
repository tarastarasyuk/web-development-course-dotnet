using System.Linq.Expressions;
using TheaterCashRegister.DAL.Models;

namespace TheaterCashRegister.DAL.Repository.IRepository;

public interface ITicketRepository : IRepository<Ticket>
{
    void Update(Ticket obj);
    Ticket Get(Expression<Func<Ticket, bool>> filter);
}