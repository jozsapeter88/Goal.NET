using AutoMapper;
using Backend.Controllers;
using Backend.DTOs;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GoalTest
{
    [TestFixture]
    public class PlayerControllerTest
    {
        private PlayerController _playerController;
        private IPlayerService _playerService;
        private GoalContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GoalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new GoalContext(options);

            _dbContext.Players.AddRange(
                new Player { Name = "TestPlayer1", Position = PositionEnum.Forward },
                new Player { Name = "TestPlayer2", Position = PositionEnum.Defender }
            );
            _dbContext.SaveChanges();

            _playerService = new PlayerService(_dbContext, Mock.Of<IMapper>());
            _playerController = new PlayerController(_playerService);
        }

        [Test]
        public async Task GetAllPlayer_ReturnListOfPlayerDto()
        {
            // Act
            var result = await _playerController.GetAllPlayers();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<PlayerDto>>(result);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}