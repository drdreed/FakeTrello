using FakeTrello.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTrello.DAL
{
    public interface IRepository
    {
        // List of methods to help deliver features
        // Create
        void AddBoard(string name, ApplicationUser owner);
        void AddList(string name, Board board);
        void AddList(string name, int boardId);
        void AddCard(string name, List list, ApplicationUser owner);
        void AddCard(string name, int listId, string ownerId);

        // Read
        List<Card> GetCardsFromList(int listId);
        List<Card> GetCardsFromBoard(int boardId);
        Card GetCard(int cardId);
        List<List> GetListsFromBoard(int boardId);
        List<Board> GetBoardsFromUser(string userId);
        Board GetBoard(int boardId);
        List<ApplicationUser> GetCardAttendees(int cardId);

        // Update
        bool AttachUser(int userId, int cardId); // true: successful, false: not successful
        bool MoveCard(int cardId, int oldListId, int newListId);
        bool CopyCard(int cardId, int newListId, string newOwnerId);

        // Delete
        bool RemoveBoard(int boardId);
        bool RemoveList(int listId);
        bool RemoveCard(int cardId);
    }
}
