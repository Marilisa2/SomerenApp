namespace SomerenApp.Models
{
    public class Room
    {
      
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomSize { get; set; }
        public RoomType RoomType { get; set; }
        public string Building { get; set; }

        public Room()
        {
        }

        public Room(int roomId, string roomNumber, string roomSize, RoomType roomType, string building)
        {
            RoomId = roomId;
            RoomNumber = roomNumber;
            RoomSize = roomSize;
            RoomType = roomType;
            Building = building;
        }
    }
}
