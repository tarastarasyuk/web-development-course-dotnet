using TheaterCashRegister.DAL.Models;

namespace TheaterCashRegister.DAL.Repository.IRepository;

public interface IBookingRepository : IRepository<Booking>
{
    void Update(Booking obj);
}