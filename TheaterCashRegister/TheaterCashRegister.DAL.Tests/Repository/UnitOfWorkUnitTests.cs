using Moq;
using TheaterCashRegister.DAL.Data;
using TheaterCashRegister.DAL.Repository;

namespace TheaterCashRegister.DAL.Tests.Repository
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        private Mock<ApplicationDbContext> _mockDbContext;
        private UnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = new Mock<ApplicationDbContext>();
            _unitOfWork = new UnitOfWork(_mockDbContext.Object);
        }

        [Test]
        public void TestUnitOfWorkCorrectlyInitializesRepositories()
        {
            Assert.That(_unitOfWork.Performance, Is.Not.Null);
            Assert.That(_unitOfWork.Ticket, Is.Not.Null);
            Assert.That(_unitOfWork.Booking, Is.Not.Null);
        }

        [Test]
        public void TestSaveCallsSaveChangesOnDbContext()
        {
            // Act
            _unitOfWork.Save();

            // Assert
            _mockDbContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}