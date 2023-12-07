namespace TheaterCashRegister.DAL.Repository.IRepository;

public interface IUnitOfWork
{
    IPerformanceRepository Performance { get; }
    ITicketRepository Ticket { get; }
    IBookingRepository Booking { get; }

    void Save();
}