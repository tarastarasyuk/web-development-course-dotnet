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
public class PerformanceServiceUnitTests
{
    private const string ErrorMessagePerformanceNotFound =
        "Error! Performance for given id was not found: performance id = {0}.";

    private const string ErrorMessagePerformanceExists =
        "Error! Performance specified title already exists: performance title = {0}.";
    
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private PerformanceService _performanceService;
    private Performance _samplePerformance;
    private PerformanceDto _samplePerformanceDto;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _performanceService = new PerformanceService(_unitOfWorkMock.Object, _mapperMock.Object);

        _samplePerformance = new Performance
        {
            Id = 1,
            Author = "Author",
            Date = DateTime.Now,
            Description = "Description",
            Genre = "Genre",
            Title = "Title"
        };

        _samplePerformanceDto = new PerformanceDto
        {
            Id = 1,
            Author = "Author",
            Date = DateTime.Now,
            Description = "Description",
            Genre = "Genre",
            Title = "Title"
        };
    }

    [Test]
    public void TestAddPerformance()
    {
        // Arrange
        _mapperMock.Setup(m => m.Map<Performance>(_samplePerformanceDto)).Returns(_samplePerformance);
        _unitOfWorkMock.Setup(u => u.Performance.GetAll(It.IsAny<Expression<Func<Performance, bool>>>())).Returns(new List<Performance>());
        _unitOfWorkMock.Setup(u => u.Performance.Add(_samplePerformance)).Verifiable();
        _unitOfWorkMock.Setup(u => u.Save()).Verifiable();
        _mapperMock.Setup(m => m.Map<PerformanceDto>(_samplePerformance)).Returns(_samplePerformanceDto);

        // Act
        var result = _performanceService.AddPerformance(_samplePerformanceDto);

        // Assert
        _unitOfWorkMock.Verify();
        _mapperMock.Verify();
        Assert.That(result, Is.EqualTo(_samplePerformanceDto));
    }
    
    [Test]
    public void TestAddPerformanceThrowsExceptionWhenPerformanceExists()
    {
        // Arrange
        _mapperMock.Setup(m => m.Map<Performance>(_samplePerformanceDto)).Returns(_samplePerformance);
        _unitOfWorkMock.Setup(u => u.Performance.GetAll(It.IsAny<Expression<Func<Performance, bool>>>())).Returns(new List<Performance> { _samplePerformance });

        // Act && Assert
        var exception = 
            Assert.Throws<EntityDuplicateException>(() => _performanceService.AddPerformance(_samplePerformanceDto));
        Assert.That(exception.Message, Is.EqualTo(string.Format(ErrorMessagePerformanceExists, _samplePerformanceDto.Title)));
    }

    [Test]
    public void TestDeletePerformance()
    {
        // Arrange
        int performanceId = _samplePerformance.Id;
        
        _unitOfWorkMock.Setup(u => u.Performance.Get(It.IsAny<Expression<Func<Performance, bool>>>()))
            .Returns(_samplePerformance);
        _unitOfWorkMock.Setup(u => u.Performance.Remove(_samplePerformance)).Verifiable();
        _unitOfWorkMock.Setup(u => u.Save()).Verifiable();

        // Act
        var result = _performanceService.DeletePerformance(performanceId);

        // Assert
        _unitOfWorkMock.Verify(u => u.Performance.Remove(_samplePerformance), Times.Once);
        _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public void TestDeleteNonExistingPerformance()
    {
        // Arrange
        int nonExistingPerformanceId = -1;

        _unitOfWorkMock.Setup(u => u.Performance.Get(It.IsAny<Expression<Func<Performance, bool>>>()))
            .Returns((Performance)null);

        // Act & Assert
        var exception =
            Assert.Throws<EntityNotFoundException>(() => _performanceService.DeletePerformance(nonExistingPerformanceId));
        Assert.That(exception?.Message, Is.EqualTo(string.Format(ErrorMessagePerformanceNotFound, nonExistingPerformanceId)));
    }

    [Test]
    public void TestSearchPerformances()
    {
        // Arrange
        var performances = new List<Performance>
        {
            _samplePerformance,
            new() { Id = 2, Author = "Author2", Title = "Title2", Genre = "Genre2", Date = DateTime.Now.AddDays(1) }
        };

        _unitOfWorkMock.Setup(u => u.Performance.GetAll(It.IsAny<Expression<Func<Performance, bool>>>()))
            .Returns(performances);

        _mapperMock.Setup(m => m.Map<PerformanceDto>(It.IsAny<Performance>()))
            .Returns<Performance>(p => new PerformanceDto { Id = p.Id });

        // Act
        var result = _performanceService.SearchPerformances(author: _samplePerformance.Author).ToList();

        // Assert
        _unitOfWorkMock.Verify();
        _mapperMock.Verify();
        Assert.That(result.Count, Is.EqualTo(performances.Count));
        Assert.That(result.First().Id, Is.EqualTo(performances.First().Id));
    }
}