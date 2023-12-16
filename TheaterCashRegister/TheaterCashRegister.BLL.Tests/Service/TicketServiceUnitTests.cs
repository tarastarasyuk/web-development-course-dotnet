using System.Linq.Expressions;
using AutoMapper;
using Moq;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.BLL.Exception;
using TheaterCashRegister.BLL.Service;
using TheaterCashRegister.DAL.Models;
using TheaterCashRegister.DAL.Repository.IRepository;

namespace TheaterCashRegister.BLL.Tests.Service;

[TestFixture]
public class TicketServiceUnitTests
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
    
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private TicketService _ticketService;
    private Ticket _sampleTicket;
    private TicketDto _sampleTicketDto;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _ticketService = new TicketService(_unitOfWorkMock.Object, _mapperMock.Object);

        _sampleTicket = new Ticket
        {
            Id = 1,
            PerformanceId = 1,
            SeatNumber = 1,
            Status = TicketStatus.Available,
        };

        _sampleTicketDto = new TicketDto
        {
            Id = 1,
            PerformanceId = 1,
            SeatNumber = 1,
            Status = "Available",
        };
    }

    [Test]
    public void TestCreateTicketWhenPerformanceExists()
    {
        // Arrange
        Performance perf = new Performance { Id = _sampleTicketDto.PerformanceId };
        _unitOfWorkMock.Setup(u => u.Performance.Get(It.IsAny<Expression<Func<Performance, bool>>>())).Returns(perf);
        _unitOfWorkMock.Setup(u => u.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns((Ticket) null);
        _mapperMock.Setup(m => m.Map<Ticket>(_sampleTicketDto)).Returns(_sampleTicket);
        _unitOfWorkMock.Setup(u => u.Ticket.Add(_sampleTicket)).Verifiable();
        _unitOfWorkMock.Setup(u => u.Save()).Verifiable();
        _mapperMock.Setup(m => m.Map<TicketDto>(_sampleTicket)).Returns(_sampleTicketDto);
        
        // Act
        var result = _ticketService.CreateTicket(_sampleTicketDto);
        
        // Assert
        _unitOfWorkMock.Verify(u => u.Ticket.Add(It.IsAny<Ticket>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        Assert.That(result, Is.EqualTo(_sampleTicketDto));
    }
    
    [Test]
    public void TestCreateTicketWhenPerformanceDoesNotExists()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Performance.Get(It.IsAny<Expression<Func<Performance, bool>>>())).Returns((Performance)null);
        
        // Act & Assert
        var exception = Assert.Throws<EntityNotFoundException>(() => _ticketService.CreateTicket(_sampleTicketDto));
    
        // Verify that Add and Save were not called
        _unitOfWorkMock.Verify(u => u.Ticket.Add(It.IsAny<Ticket>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Never);
        StringAssert.Contains(string.Format(ErrorMessageNotValidPerformanceId, _sampleTicketDto.PerformanceId), exception.Message);
    }
    
    [Test]
    public void TestCreateTicketThrowsExceptionWhenTicketExists()
    {
        // Arrange
        Performance perf = new Performance { Id = _sampleTicketDto.PerformanceId };
        _unitOfWorkMock.Setup(u => u.Performance.Get(It.IsAny<Expression<Func<Performance, bool>>>())).Returns(perf);
        // Mock unitOfWork.Ticket.Get to return a ticket
        _unitOfWorkMock.Setup(t => t.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns(_sampleTicket);
        
        // Act && Assert
        var ex = Assert.Throws<EntityDuplicateException>(() => _ticketService.CreateTicket(_sampleTicketDto));
        Assert.That(ex.Message, Is.EqualTo(
            string.Format(ErrorMessageDuplicateSeatNumber, _sampleTicketDto.SeatNumber, _sampleTicketDto.PerformanceId)));
    }

    [Test]
    public void TestGetTicket()
    {
        // Arrange
        int seatNumber = _sampleTicket.SeatNumber;
        int performanceId = _sampleTicket.PerformanceId;

        _unitOfWorkMock.Setup(u => u.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(_sampleTicket);
        _mapperMock.Setup(m => m.Map<TicketDto>(_sampleTicket)).Returns(_sampleTicketDto);

        // Act
        var result = _ticketService.GetTicket(seatNumber, performanceId);

        // Assert
        _unitOfWorkMock.Verify();
        _mapperMock.Verify();
        Assert.That(result, Is.EqualTo(_sampleTicketDto));
    }
    
    [Test]
    public void TestGetTicketWhenTicketNotFound()
    {
        // Arrange
        int seatNumber = -1;
        int performanceId = -1;

        _unitOfWorkMock.Setup(u => u.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns((Ticket)null);

        // Act & Assert
        var ex = Assert.Throws<EntityNotFoundException>(() => _ticketService.GetTicket(seatNumber, performanceId));
        StringAssert.Contains(string.Format(ErrorMessageTicketNotFound, seatNumber, performanceId), ex.Message);
    }

    [Test]
    public void TestBuyTicketWhenTicketIsAvailable()
    {
        // Arrange
        int seatNumber = _sampleTicket.SeatNumber;
        int performanceId = _sampleTicket.PerformanceId;

        _unitOfWorkMock.Setup(u => u.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(_sampleTicket);
        _unitOfWorkMock.Setup(u => u.Ticket.Update(It.IsAny<Ticket>())).Verifiable();
        _unitOfWorkMock.Setup(u => u.Save()).Verifiable();
        _mapperMock.Setup(m => m.Map<TicketDto>(_sampleTicket)).Returns(_sampleTicketDto);

        // Act
        var result = _ticketService.BuyTicket(seatNumber, performanceId);

        // Assert
        _unitOfWorkMock.Verify();
        _mapperMock.Verify();
        Assert.That(result, Is.EqualTo(_sampleTicketDto));
        Assert.That(_sampleTicket.Status, Is.EqualTo(TicketStatus.Sold));
    }

    [Test]
    public void TestBuyTicketWhenTicketIsNotAvailable()
    {
        // Arrange
        int seatNumber = _sampleTicket.SeatNumber;
        int performanceId = _sampleTicket.PerformanceId;

        _unitOfWorkMock.Setup(u => u.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(_sampleTicket);

        _sampleTicket.Status = TicketStatus.Sold;

        // Act & Assert
        var ex = Assert.Throws<EntityIllegalStateException>(() => _ticketService.BuyTicket(seatNumber, performanceId));
        StringAssert.Contains(string.Format(ErrorMessageTicketIsNotAvailable, seatNumber, performanceId), ex.Message);

        _sampleTicket.Status = TicketStatus.Available; // Reset status for other tests
    }
    
    [Test]
    public void TestBuyTicketWhenTicketNotFound()
    {
        // Arrange
        int seatNumber = -1;
        int performanceId = -1;

        _unitOfWorkMock.Setup(u => u.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns((Ticket)null);

        // Act & Assert
        var ex = Assert.Throws<EntityNotFoundException>(() => _ticketService.BuyTicket(seatNumber, performanceId));
        StringAssert.Contains(string.Format(ErrorMessageTicketNotFound, seatNumber, performanceId), ex.Message);
    }

    [Test]
    public void TestBookTicketWhenTicketIsAvailable()
    {
        // Arrange
        int seatNumber = _sampleTicket.SeatNumber;
        int performanceId = _sampleTicket.PerformanceId;

        _unitOfWorkMock.Setup(u => u.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(_sampleTicket);
        _unitOfWorkMock.Setup(u => u.Ticket.Update(It.IsAny<Ticket>())).Verifiable();
        _unitOfWorkMock.Setup(u => u.Booking.Add(It.IsAny<Booking>())).Verifiable();
        _unitOfWorkMock.Setup(u => u.Save()).Verifiable();
        _mapperMock.Setup(m => m.Map<TicketDto>(_sampleTicket)).Returns(_sampleTicketDto);

        // Act
        var (result, uuid) = _ticketService.BookTicket(seatNumber, performanceId);

        // Assert
        _unitOfWorkMock.Verify();
        _mapperMock.Verify();
        Assert.That(result, Is.EqualTo(_sampleTicketDto));
        Assert.That(_sampleTicket.Status, Is.EqualTo(TicketStatus.Booked));
    }

    [Test]
    public void TestBookTicketWhenTicketIsNotAvailable()
    {
        // Arrange
        int seatNumber = _sampleTicket.SeatNumber;
        int performanceId = _sampleTicket.PerformanceId;

        _unitOfWorkMock.Setup(u => u.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns(_sampleTicket);
        _sampleTicket.Status = TicketStatus.Sold;

        // Act & Assert
        var ex = Assert.Throws<EntityIllegalStateException>(() => _ticketService.BookTicket(seatNumber, performanceId));
        StringAssert.Contains(string.Format(ErrorMessageTicketIsNotAvailable, seatNumber, performanceId), ex.Message);
    }

    [Test]
    public void TestConfirmBookedTicket()
    {
        // Arrange
        Guid testUuid = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Booking.Get(It.IsAny<Expression<Func<Booking, bool>>>()))
            .Returns(new Booking { UUID = testUuid });
        _unitOfWorkMock.Setup(u => u.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(_sampleTicket);
        _unitOfWorkMock.Setup(u => u.Ticket.Update(It.IsAny<Ticket>())).Verifiable();
        _unitOfWorkMock.Setup(u => u.Save()).Verifiable();
        _mapperMock.Setup(m => m.Map<TicketDto>(_sampleTicket)).Returns(_sampleTicketDto);

        _sampleTicket.Status = TicketStatus.Booked;

        // Act
        var result = _ticketService.ConfirmBookedTicket(testUuid);

        // Assert
        _unitOfWorkMock.Verify();
        _mapperMock.Verify();
        Assert.That(result, Is.EqualTo(_sampleTicketDto));
        Assert.That(_sampleTicket.Status, Is.EqualTo(TicketStatus.Sold));
    }

    [Test]
    public void TestConfirmBookedTicketWhenBookingNotFound()
    {
        // Arrange
        Guid nonExistingUuid = Guid.NewGuid();

        _unitOfWorkMock.Setup(u => u.Booking.Get(It.IsAny<Expression<Func<Booking, bool>>>()))
            .Returns((Booking)null);

        // Act & Assert
        var ex = Assert.Throws<EntityNotFoundException>(() => _ticketService.ConfirmBookedTicket(nonExistingUuid));
        StringAssert.Contains(string.Format(ErrorMessageBookingNotFound, nonExistingUuid), ex.Message);
    }

    [Test]
    public void TestConfirmBookedTicketWhenTicketNotBooked()
    {
        // Arrange
        Guid testUuid = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Booking.Get(It.IsAny<Expression<Func<Booking, bool>>>()))
            .Returns(new Booking { UUID = testUuid });
        _unitOfWorkMock.Setup(u => u.Ticket.Get(It.IsAny<Expression<Func<Ticket, bool>>>()))
            .Returns(_sampleTicket);

        _sampleTicket.Status = TicketStatus.Sold;

        // Act & Assert
        var ex = Assert.Throws<EntityIllegalStateException>(() => _ticketService.ConfirmBookedTicket(testUuid));
        StringAssert.Contains(string.Format(ErrorMessageTicketNotBookedWithUuid, testUuid), ex.Message);
    }

}