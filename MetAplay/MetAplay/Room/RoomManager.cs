using Google.Protobuf.Protocol;


namespace MetAplay
{
    public class RoomManager
    {
        public static RoomManager Instance { get; } = new RoomManager();

        object _lock = new object();
        Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();

        public List<GameRoom> Rooms
        {
            get
            {
                List<GameRoom> list = new List<GameRoom>();
                foreach (GameRoom r in _rooms.Values)
                {
                    list.Add(r);
                }
                return list;
            }
        }
        public int Count { get { return _rooms.Count; } }
        int _roomId = 1;

        public GameRoom Add(RoomSetting setting)
        {
            GameRoom gameRoom = new GameRoom();

            lock (_lock)
            {
                gameRoom.RoomId = _roomId;
                gameRoom.Setting = setting;
                _rooms.Add(_roomId, gameRoom);
                _roomId++;
            }

            return gameRoom;
        }

        public bool Remove(int roomId)
        {
            lock (_lock)
            {
                return _rooms.Remove(roomId);
            }
        }

        public GameRoom Find(int roomId)
        {
            lock (_lock)
            {
                GameRoom room = null;
                if (_rooms.TryGetValue(roomId, out room))
                    return room;

                return null;
            }
        }
    }
}