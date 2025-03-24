using SomerenApp.Models;
using System.Collections.Generic;

namespace SomerenApp.Repositories
{
    public interface IRoomsRepository
    {
        List<Room> GetAllRooms();
        Room? GetByID(int roomId); //specific room
        void Add(Room room);
        void Update(Room room);
        void Delete(Room room);

        //method to filter rooms based on the number of beds/ roomsize
        List<Room> GetRoomsBySize(string roomSize);
    }
}
