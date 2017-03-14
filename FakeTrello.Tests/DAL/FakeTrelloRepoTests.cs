using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeTrello.DAL;
using Moq;
using FakeTrello.Models;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace FakeTrello.Tests.DAL
{
    [TestClass]
    public class FakeTrelloRepoTests
    {
        public Mock<FakeTrelloContext> fake_context { get; set; }
        public FakeTrelloRepository repo { get; set; }
        public Mock<DbSet<Board>> mock_boards_set { get; set; }
        public IQueryable<Board> query_boards { get; set; }
        public List<Board> fake_board_table { get; set; }
        public ApplicationUser sally { get; set; }
        public ApplicationUser sammy { get; set; }

        [TestInitialize]
        public void Setup()
        {
            fake_board_table = new List<Board>();
            fake_context = new Mock<FakeTrelloContext>();
            mock_boards_set = new Mock<DbSet<Board>>();
            repo = new FakeTrelloRepository(fake_context.Object);

            sally = new ApplicationUser { Id = "sally-id-1" };
            sammy = new ApplicationUser { Id = "sammy-id-1" };
        }

        public void CreateFakeDatabase()
        {
            query_boards = fake_board_table.AsQueryable(); // Re-express this list as something that understands queries
            
            // Hey LINQ, use the Provider from our *cough* fake *cough* board table/list
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.Provider).Returns(query_boards.Provider);
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.Expression).Returns(query_boards.Expression);
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.ElementType).Returns(query_boards.ElementType);
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.GetEnumerator()).Returns(() => query_boards.GetEnumerator());

            mock_boards_set.Setup(b => b.Add(It.IsAny<Board>())).Callback((Board board) => fake_board_table.Add(board));
            mock_boards_set.Setup(b => b.Remove(It.IsAny<Board>())).Callback((Board board) => fake_board_table.Remove(board));
            fake_context.Setup(c => c.Boards).Returns(mock_boards_set.Object); // Context.Boards returns fake_board_table (a list)
        }

        [TestMethod]
        public void EnsureICanCreateInstanceofRepo()
        {
            FakeTrelloRepository repo = new FakeTrelloRepository();

            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void EnsureIHaveNotNullContext()
        {
            FakeTrelloRepository repo = new FakeTrelloRepository();

            Assert.IsNotNull(repo.Context);
        }

        [TestMethod]
        public void EnsureICanInjectContextInstance()
        {
            FakeTrelloContext context = new FakeTrelloContext();
            FakeTrelloRepository repo = new FakeTrelloRepository(context);

            Assert.IsNotNull(repo.Context);
        }

        [TestMethod]
        public void EnsureICanAddBoard()
        {
            // Arrange
            CreateFakeDatabase();

            ApplicationUser a_user = new ApplicationUser {
                Id = "my-user-id",
                UserName = "Sammy",
                Email = "sammy@gmail.com"
            };

            // Act
            repo.AddBoard("My Board", a_user);

            // Assert
            Assert.AreEqual(1, repo.Context.Boards.Count());
        }

        [TestMethod]
        public void EnsureICanReturnBoards()
        {
            // Arrange
            fake_board_table.Add(new Board { Name = "My Board" });
            CreateFakeDatabase();

            // Act
            int expected_board_count = 1;
            int actual_board_count = repo.Context.Boards.Count();

            // Assert
            Assert.AreEqual(expected_board_count, actual_board_count);
        }

        [TestMethod]
        public void EnsureICanFindABoard()
        {
            // Arrange
            fake_board_table.Add(new Board { BoardId = 1, Name = "My Board" });
            CreateFakeDatabase();

            // Act
            string expected_board_name = "My Board";
            Board actual_board = repo.GetBoard(1);
            string actual_board_name = repo.GetBoard(1).Name;

            // Assert
            Assert.IsNotNull(actual_board);
            Assert.AreEqual(expected_board_name, actual_board_name);
        }

        [TestMethod]
        public void EnsureICanGetUserBoards()
        {
            
            fake_board_table.Add(new Board { BoardId = 1, Name = "My Board", Owner = sally });
            fake_board_table.Add(new Board { BoardId = 2, Name = "My Board", Owner = sally });
            fake_board_table.Add(new Board { BoardId = 3, Name = "My Board", Owner = sammy });
            CreateFakeDatabase();

            int expected_board_count = 2;
            int actual_board_count = repo.GetBoardsFromUser(sally.Id).Count;

            Assert.AreEqual(expected_board_count, actual_board_count);

        }

        [TestMethod]
        public void EnsureICanRemoveBoard()
        {
            fake_board_table.Add(new Board { BoardId = 1, Name = "My Board", Owner = sally });
            fake_board_table.Add(new Board { BoardId = 2, Name = "My Board", Owner = sally });
            fake_board_table.Add(new Board { BoardId = 3, Name = "My Board", Owner = sammy });
            CreateFakeDatabase();

            int expected_board_count = 2;
            repo.RemoveBoard(3);
            int actual_board_count = repo.Context.Boards.Count();

            Assert.AreEqual(expected_board_count, actual_board_count);
        }
    }
}
