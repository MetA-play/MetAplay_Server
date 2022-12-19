using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            // 방 오브젝트 생성
            RoomObject roomObj = ObjectManager.Instance.Add<RoomObject>();
            GameRoom room = RoomManager.Instance.Add(setting);
            roomObj.Room = room;
            roomObj.Info.Transform.Pos = player.Info.Transform.Pos;
            _roomObjs.Add(roomObj.Id, roomObj);

            EnterGame(roomObj);

            // 방 들어가기
            roomObj.Enter(player,isHost:true);
            S_CreateroomRes res = new S_CreateroomRes();
            res.RoomId = room.RoomId;
            player.Session.Send(res);
            LeaveGame(player.Id);

        }

        public void JoinRoomHandle(int roomId,Player player)
        {
            // 방 들어가기
            GameRoom room = RoomManager.Instance.Find(roomId);
            if (room.IsStart) return;
            room.EnterGame(player);

            // 정보 전송
            S_JoinroomRes res = new S_JoinroomRes();
            res.RoomId = roomId;
            player.Session.Send(res);
        }
        public void DeleteRoom()
        {

        }
        public override void EnterGame(GameObject gameObject)
        {

            if (gameObject.ObjectType == GameObjectType.Player)
            {
                Player player = gameObject as Player;
                _players.Add(gameObject.Id, player);

                {
                    S_EnterGame entergame = new S_EnterGame();
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
            if (type == GameObjectType.Player)
            {
                Player player = null;
                if (_players.TryGetValue(gameObjectId, out player) == false)
                    return;

                player.Room = null;

                _players.Remove(gameObjectId);

                {
                    S_LeaveGame leave = new S_LeaveGame();
                    player.Session.Send(leave);
                }
            }


            {

                S_Despawn despawn = new S_Despawn();
                despawn.ObjectId.Add(gameObjectId);

                foreach (Player p in _players.Values)
                {
                    if(p.Id != gameObjectId)
                        p.Session.Send(despawn);
                }
            }

        }
    }
}
