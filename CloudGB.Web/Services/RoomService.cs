namespace CloudGB.Web.Services
{
    public class RoomService
    {
        private readonly Dictionary<long, RoomSession> _roomSessions = [];

        public RoomSession? GetRoomSession(long roomId)
        {
            return _roomSessions[roomId];
        }
    }
}
