using TheaterCashRegister.DAL.Data;
using TheaterCashRegister.DAL.Repository.IRepository;

namespace TheaterCashRegister.DAL.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public IPerformanceRepository Performance { get; }
    public ITicketRepository Ticket { get; }
    public IBookingRepository Booking { get; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        Performance = new PerformanceRepository(_dbContext);
        Ticket = new TicketRepository(_dbContext);
        Booking = new BookingRepository(_dbContext);
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}