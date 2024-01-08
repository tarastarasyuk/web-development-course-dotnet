using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TheaterCashRegister.DAL.Data;
using TheaterCashRegister.DAL.Models;
using TheaterCashRegister.DAL.Repository.IRepository;

namespace TheaterCashRegister.DAL.Repository;

public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    private ApplicationDbContext _dbContext;

    public TicketRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Ticket obj)
    {
        _dbContext.Update(obj);
    }

    public Ticket Get(Expression<Func<Ticket, bool>> filter)
    {
        return _dbContext.Ticket
            .Include(t => t.Booking)
            .Where(filter)
            .FirstOrDefault();
    }
}