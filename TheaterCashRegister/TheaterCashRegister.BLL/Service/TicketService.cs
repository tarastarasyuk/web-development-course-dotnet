using AutoMapper;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.BLL.Exception;
using TheaterCashRegister.BLL.Service.IService;
using TheaterCashRegister.DAL.Models;
using TheaterCashRegister.DAL.Repository.IRepository;

namespace TheaterCashRegister.BLL.Service;

public class TicketService : ITicketService
{
    private const string ErrorMessageTicketNotFound =
        "Error! Ticket for given parameters was not found: seat number = {0} and performance id = {1}.";

    private const string ErrorMessageTicketIsNotAvailable =
        "Error! Ticket for given parameters is not 'Available': seat number = {0} and performance id = {1}.";

    private const string ErrorMessageBookingNotFound =
        "Error! Booking not found for the given UUID: {0}.";

    private const string ErrorMessageTicketNotBookedWithUuid =
        "Error! Ticket associated with the given UUID {0} is not 'Booked'.";

    private const string ErrorMessageNotValidPerformanceId =
        "Error! Ticket can not be assign to non-existent performance: performance id = {0}";
    
    private const string ErrorMessageDuplicateSeatNumber =
        "Error! Ticket with specified seat number already exists for the performance: seat number = {0}, performance id = {1}";
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public TicketDto CreateTicket(TicketDto ticketDto)
    {
        var ticketPerformanceId = ticketDto.PerformanceId;
        if (_unitOfWork.Performance.Get(p => p.Id == ticketPerformanceId) == null) 
        {
            throw new EntityNotFoundException(string.Format(ErrorMessageNotValidPerformanceId, ticketPerformanceId));
        }

        var ticketSeatNumber = ticketDto.SeatNumber;
        if ( _unitOfWork.Ticket.Get(t => t.SeatNumber == ticketSeatNumber && t.PerformanceId == ticketPerformanceId) !=  null)
        {
            throw new EntityDuplicateException(string.Format(ErrorMessageDuplicateSeatNumber, ticketSeatNumber, ticketPerformanceId));
        }
        
        var ticket = _mapper.Map<Ticket>(ticketDto);
        _unitOfWork.Ticket.Add(ticket);
        _unitOfWork.Save();
        return _mapper.Map<TicketDto>(ticket);
    }

    public TicketDto GetTicket(int seatNumber, int performanceId)
    {
        var ticket = GetTicketInternal(seatNumber, performanceId);
        return _mapper.Map<TicketDto>(ticket);
    }

    public TicketDto BuyTicket(int seatNumber, int performanceId)
    {
        var ticket = GetTicketInternal(seatNumber, performanceId);

        if (ticket.Status != TicketStatus.Available)
        {
            throw new EntityIllegalStateException(string.Format(ErrorMessageTicketIsNotAvailable, seatNumber, performanceId));
        }

        ticket.Status = TicketStatus.Sold;
        _unitOfWork.Ticket.Update(ticket);
        _unitOfWork.Save();
        return _mapper.Map<TicketDto>(ticket);
    }

    public (TicketDto, Guid) BookTicket(int seatNumber, int performanceId)
    {
        var ticket = GetTicketInternal(seatNumber, performanceId);

        if (ticket.Status != TicketStatus.Available)
        {
            throw new EntityIllegalStateException(string.Format(ErrorMessageTicketIsNotAvailable, seatNumber, performanceId));
        }

        ticket.Status = TicketStatus.Booked;
        _unitOfWork.Ticket.Update(ticket);

        DateTime expirationDate = DateTime.UtcNow.AddHours(2);
        var booking = new Booking { TicketId = ticket.Id, ExpirationDate = expirationDate, UUID = Guid.NewGuid() };
        _unitOfWork.Booking.Add(booking);

        _unitOfWork.Save();
        var ticketDto = _mapper.Map<TicketDto>(ticket);
        return (ticketDto, booking.UUID);
    }

    public TicketDto ConfirmBookedTicket(Guid uuid)
    {
        var booking = _unitOfWork.Booking.Get(b => b.UUID == uuid);

        if (booking == null)
        {
            throw new EntityNotFoundException(string.Format(ErrorMessageBookingNotFound, uuid));
        }

        var ticket = _unitOfWork.Ticket.Get(t => t.Id == booking.TicketId);

        if (ticket.Status != TicketStatus.Booked)
        {
            throw new EntityIllegalStateException(string.Format(ErrorMessageTicketNotBookedWithUuid, uuid));
        }

        ticket.Status = TicketStatus.Sold;
        _unitOfWork.Ticket.Update(ticket);
        _unitOfWork.Booking.Remove(booking);
        _unitOfWork.Save();
        return _mapper.Map<TicketDto>(ticket);
    }

    private Ticket GetTicketInternal(int seatNumber, int performanceId)
    {
        var ticket = _unitOfWork.Ticket.Get(t => t.SeatNumber == seatNumber && t.PerformanceId == performanceId);

        if (ticket == null)
        {
            throw new EntityNotFoundException(string.Format(ErrorMessageTicketNotFound, seatNumber, performanceId));
        }

        return ticket;
    }
}