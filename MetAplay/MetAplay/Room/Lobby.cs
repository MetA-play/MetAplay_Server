using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class Lobby : BaseRoom
    {
        public static Lobby Instance { get; private set; } = new Lobby();

        Dictionary<int, RoomObject> _roomObjs = new Dictionary<int, RoomObject>();

        public void CreateRoomHandle(Player player, RoomSetting setting)
        {
            RoomObject roomObj = ObjectManager.Instance.Add<RoomObject>();
            GameRoom room = RoomManager.Instance.Add(setting);
            roomObj.Room = room;
            roomObj.Info.Transform.Pos = player.Info.Transform.Pos;
            _roomObjs.Add(roomObj.Id, roomObj);
            EnterGame(roomObj);

            S_CreateRoomRes res = new S_CreateRoomRes();
            res.RoomId = room.RoomId;
            res.ObjectId = roomObj.Id;
            player.Session.Send(res);
            LeaveGame(player.Id);
        }

        public void JoinRoomHandle(int roomId, Player player)
        {
            GameRoom room = RoomManager.Instance.Find(roomId);

            if(room == null)
            {
                Console.WriteLine("room is null");
                return;
            }
            if (room.IsStart) return;

            if (room.Host == null)
                room.Host = player;

            if (room.Players.Contains(player) == true)
                return;

            S_JoinRoomRes res = new S_JoinRoomRes();
            res.RoomId = roomId;
            player.Session.Send(res);
         
            room.EnterGame(player);
        }

        public void DeleteRoom()
        {

        }

        public override void EnterGame(GameObject gameObject)
        {
            if (gameObject.ObjectType.Equals(GameObjectType.Player))
            {
                Player player = gameObject as Player;
                _players.Add(gameObject.Id, player);

                {
                    S_EnterGame entergame = new S_EnterGame();
                    entergame.Player = gameObject.Info;
                    player.Session.Send(entergame);
                    S_Spawn spawn = new S_Spawn();

                    foreach (Player p in _players.Values)
                    {
                        if (player != p)
                            spawn.Objects.Add(p.Info);
                    }
                    foreach (RoomObject obj in _roomObjs.Values)
                        spawn.Objects.Add(obj.Info);

                    player.Session.Send(spawn);
                }
            }

            {


                S_Spawn spawn = new S_Spawn();

                if (gameObject.ObjectType.Equals(GameObjectType.Room))
                {
                    RoomObject roomObj = gameObject as RoomObject;
                    roomObj.Info.Transform.Scale.Y = roomObj.Room.RoomId;
                    spawn.Objects.Add(roomObj.Info);
                }
                else
                    spawn.Objects.Add(gameObject.Info);

                foreach (Player p in _players.Values)
                {
                    if (p.Id != gameObject.Id)
                        p.Session.Send(spawn);
                }
            }
        }

        public override void LeaveGame(int gameObjectId)
        {
            GameObjectType type = ObjectManager.GetObjectTypeById(gameObjectId);

            if (type.Equals(GameObjectType.Player))
            {
                if (!_players.TryGetValue(gameObjectId, out Player player)) return;

                player.Room = null;
                _players.Remove(gameObjectId);

                S_LeaveGame leave = new S_LeaveGame();
                player.Session.Send(leave);
            }

            S_Despawn despawn = new S_Despawn();
            despawn.ObjectId.Add(gameObjectId);

            foreach (Player p in _players.Values)
            {
                if (p.Id != gameObjectId)
                    p.Session.Send(despawn);
            }
        }
    }
}
