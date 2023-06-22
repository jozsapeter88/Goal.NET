using AutoMapper;
using Backend.DTOs;
using Backend.DTOs.AutoMapper;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace GoalTest
{
    [TestFixture]
    public class PlayerServiceTest
    {
        private PlayerService _playerService;
        private GoalContext _dbContext;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new AutoMapperProfile()); });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;

            var options = new DbContextOptionsBuilder<GoalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new GoalContext(options);

            _dbContext.Players.AddRange(
                new Player { Name = "TestForward", Position = PositionEnum.Forward },
                new Player { Name = "TestDefender", Position = PositionEnum.Defender },
                new Player { Name = "TestGoalkeeper", Position = PositionEnum.Goalkeeper },
                new Player { Name = "TestMidfielder", Position = PositionEnum.Midfielder }
            );
            _dbContext.SaveChanges();

            _playerService = new PlayerService(_dbContext, _mapper);
        }

        [Test]
        public async Task GetAllPlayers_ShouldReturnAllPlayer()
        {
            // Act
            var result = await _playerService.GetAllPlayers();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Any(player => player.Name == "TestForward"), Is.True);
            Assert.That(result.Any(player => player.Name == "TestDefender"), Is.True);
            Assert.That(result.Any(player => player.Name == "TestGoalkeeper"), Is.True);
            Assert.That(result.Any(player => player.Name == "TestMidfielder"), Is.True);
        }

        [Test]
        public async Task GetForwards_ShouldReturnGetForwards()
        {
            // Act
            var result = await _playerService.GetForwards();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("TestForward"));
        }

        [Test]
        public async Task GetGoalkeepers_ShouldReturnGoalkeepers()
        {
            // Act
            var result = await _playerService.GetGoalKeepers();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("TestGoalkeeper"));
        }

        [Test]
        public async Task GetMidfielders_ShouldReturnMidfielders()
        {
            // Act
            var result = await _playerService.GetMidfielders();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("TestMidfielder"));
        }

        [Test]
        public async Task GetDefenders_ShouldReturnDefenders()
        {
            // Act
            var result = await _playerService.GetDefenders();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("TestDefender"));
        }

        [Test]
        public async Task CreatePlayerByAdmin_ShouldCreateNewPlayer()
        {
            // Arrange
            var playerDto = new PlayerDto
            {
                Name = "CreatedTestPlayer",
                Position = PositionEnum.Forward
            };

            // Act
            var createdPlayer = await _playerService.CreatePlayerByAdmin(playerDto);

            // Assert
            Assert.NotNull(createdPlayer);
            Assert.That(createdPlayer.Name, Is.EqualTo("CreatedTestPlayer"));
            Assert.That(createdPlayer.Position, Is.EqualTo(PositionEnum.Forward));
        }

        [Test]
        public async Task DeletePlayer_ShouldDeleteExistingPlayer()
        {
            // Arrange
            long playerIdToDelete = 1;

            // Act
            var deletedPlayers = await _playerService.DeletePlayer(playerIdToDelete);
            var remainingPlayers = await _dbContext.Players.ToListAsync();

            // Assert
            Assert.NotNull(deletedPlayers);
            Assert.IsFalse(remainingPlayers.Any(p => p.Id == playerIdToDelete));
        }


        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}