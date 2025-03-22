using SomerenApp.Models;
using System.Collections.Generic;

namespace SomerenApp.Repositories
{
    public interface IRoomsRepository
    {
        List<Room> GetAllRooms();
        Room? GetByID(int roomId); //specifieke room
        void Add(Room room);
        void Update(Room room);
        void Delete(Room room);

        //methode om kamers te filteren op basis va het aantal bedden
        List<Room> GetRoomsBySize(string roomSize);
    }
}
