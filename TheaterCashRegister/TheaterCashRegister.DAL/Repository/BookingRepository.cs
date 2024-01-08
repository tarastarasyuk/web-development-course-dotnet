using TheaterCashRegister.DAL.Data;
using TheaterCashRegister.DAL.Models;
using TheaterCashRegister.DAL.Repository.IRepository;

namespace TheaterCashRegister.DAL.Repository;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    private ApplicationDbContext _dbContext;

    public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Booking obj)
    {
        _dbContext.Update(obj);
    }
}