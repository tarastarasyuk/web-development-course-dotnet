using Microsoft.EntityFrameworkCore;
using Moq;
using TheaterCashRegister.DAL.Data;
using TheaterCashRegister.DAL.Repository;

namespace TheaterCashRegister.DAL.Tests.Repository;

[TestFixture]
public class RepositoryTests
{
    private Mock<ApplicationDbContext> _mockDbContext;
    private Mock<DbSet<SampleEntity>> _mockDbSet;
    private Repository<SampleEntity> _repository;

    public class SampleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [SetUp]
    public void Setup()
    {
        var data = new List<SampleEntity>()
        {
            new() { Id = 1, Name = "Entity1" },
            new() { Id = 2, Name = "Entity2" },
            new() { Id = 3, Name = "Entity3" },
        }.AsQueryable();

        _mockDbSet = new Mock<DbSet<SampleEntity>>();
        _mockDbSet.As<IQueryable<SampleEntity>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockDbSet.As<IQueryable<SampleEntity>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockDbSet.As<IQueryable<SampleEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockDbSet.As<IQueryable<SampleEntity>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _mockDbContext = new Mock<ApplicationDbContext>();
        _mockDbContext.Setup(x => x.Set<SampleEntity>()).Returns(_mockDbSet.Object);

        _repository = new Repository<SampleEntity>(_mockDbContext.Object);
    }

    [Test]
    public void TestGetAllReturnsAllEntities()
    {
        var result = _repository.GetAll();

        Assert.That(result.Count(), Is.EqualTo(3));
    }

    [Test]
    public void TestGetReturnsSingleEntityByFilter()
    {
        var result = _repository.Get(x => x.Id == 1);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Entity1"));
        });
    }

    [Test]
    public void TestGetAllWithFilterReturnsFilteredResult()
    {
        var result = _repository.GetAll(x => x.Id > 1).ToList();

        Assert.That(result.Count, Is.EqualTo(2));
        foreach (var item in result)
        {
            Assert.That(item.Id, Is.GreaterThan(1));
        }
    }

    [Test]
    public void TestAddCallsAddOnDbSet()
    {
        var entity = new SampleEntity();

        _repository.Add(entity);

        _mockDbSet.Verify(x => x.Add(entity), Times.Once());
    }

    [Test]
    public void TestRemoveCallsRemoveOnDbSet()
    {
        var entity = new SampleEntity();

        _repository.Remove(entity);

        _mockDbSet.Verify(x => x.Remove(entity), Times.Once());
    }

    [Test]
    public void TestRemoveRangeCallsRemoveRangeOnDbSet()
    {
        var entities = new List<SampleEntity>
        {
            new(),
            new()
        };

        _repository.RemoveRange(entities);

        _mockDbSet.Verify(x => x.RemoveRange(entities), Times.Once());
    }
}