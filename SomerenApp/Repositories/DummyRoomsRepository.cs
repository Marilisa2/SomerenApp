using SomerenApp.Models;
namespace SomerenApp.Repositories
{
    public class DummyRoomsRepository : IRoomsRepository
    {
        List<Room> rooms =
        [
            new Room(1, "A1-01", "Ground floor", RoomType.Lecturer, "A"),
            new Room(2, "A1-02", "Ground floor", RoomType.Lecturer, "A"),
            new Room(3, "B1-01", "8 beds", RoomType.Student, "B"),
            new Room(4, "B1-02", "8 beds", RoomType.Student, "B"),
            new Room(5, "B1-03", "8 beds", RoomType.Student, "B"),
        ];

        public void Add(Room room)
        {
            throw new NotImplementedException();
        }

        public void Delete(Room room)
        {
            throw new NotImplementedException();
        }

        public List<Room> GetAllRooms()
        {
            return rooms;
        }

        public Room? GetByID(int roomId)
        {
            return rooms.FirstOrDefault(x => x.RoomId == roomId);
        }

        public List<Room> GetRoomsBySize(string roomSize)
        {
            throw new NotImplementedException();
        }

        public void Update(Room room)
        {
            throw new NotImplementedException();
        }
    }
}
