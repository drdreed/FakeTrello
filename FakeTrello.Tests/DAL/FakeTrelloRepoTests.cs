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
        public Mock<FakeTrelloContext> fakeContext { get; set; }
        public FakeTrelloRepository repo { get; set; }
        public Mock<DbSet<Board>> mockBoardsSet { get; set; }
        public IQueryable<Board> queryBoards { get; set; }
        public List<Board> fakeBoardTable { get; set; }

        [TestInitialize]
        public void Setup()
        {
            fakeBoardTable = new List<Board>();
            fakeContext = new Mock<FakeTrelloContext>();
            mockBoardsSet = new Mock<DbSet<Board>>();
            repo = new FakeTrelloRepository(fakeContext.Object);
        }

        public void CreateFakeDatabase()
        {

            var queryBoards = fakeBoardTable.AsQueryable();

            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.Provider).Returns(queryBoards.Provider);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.Expression).Returns(queryBoards.Expression);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.ElementType).Returns(queryBoards.ElementType);
            mockBoardsSet.As<IQueryable<Board>>().Setup(b => b.GetEnumerator()).Returns(queryBoards.GetEnumerator());

            mockBoardsSet.Setup(b => b.Add(It.IsAny<Board>())).Callback((Board board) => fakeBoardTable.Add(board));
            fakeContext.Setup(c => c.Boards).Returns(mockBoardsSet.Object);
        }

        [TestMethod]
        public void EnsureICanCreateInstanceofRepo()
        {
            FakeTrelloRepository repo = new FakeTrelloRepository();

            Assert.IsNotNull(repo);
        }


        [TestMethod]
        public void EnsureICanInjectContestInstance()
        {
            FakeTrelloContext context = new FakeTrelloContext();

            FakeTrelloRepository repo = new FakeTrelloRepository(context);

            Assert.IsNotNull(repo.Context);
        }

        [TestMethod]
        public void EnsureICanHaveNotNullContext()
        {
            FakeTrelloRepository repo = new FakeTrelloRepository();

            Assert.IsNotNull(repo.Context);
        }

        [TestMethod]
        public void EnsureICanAddBoard()
        {
            // Arrange
            CreateFakeDatabase();
            
            ApplicationUser aUser = new ApplicationUser
            {
                Id = "my-user-id",
                UserName = "Sammy",
                Email = "sammy@gmail.com"
            };

            // Act
            repo.AddBoard("My Board", aUser);

            // Assert
            Assert.AreEqual(repo.Context.Boards.Count(), 1);
        }

        [TestMethod]
        public void EnsureICanReturnBoards()
        {
            // Arrange
            fakeBoardTable.Add(new Board { Name = "My Board" });
            CreateFakeDatabase();

            // Act

            int expectedBoardCount = 1;
            int actualBoardCount = repo.Context.Boards.Count();

            // Assert
            Assert.AreEqual(expectedBoardCount, actualBoardCount);
        }

        [TestMethod]
        public void EnsureICanFindABoard()
        {
            fakeBoardTable.Add(new Board { BoardId = 1, Name = "My Board" });
            CreateFakeDatabase();

            // Act

            string expectedBoardName = "My Board";
            string actualBoardName = repo.GetBoard(1).Name;

            // Assert
            Assert.AreEqual(expectedBoardName, actualBoardName);

        }
    }
}
