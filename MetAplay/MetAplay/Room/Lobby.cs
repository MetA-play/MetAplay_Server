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

        Dictionary<int, RoomObject> _roomObjs = new Dictionary<int, RoomObject>();
        public void CreateRoomHandle(ClientSession session, RoomSetting setting)
        {

            RoomObject roomObj = ObjectManager.Instance.Add<RoomObject>();
            GameRoom room = RoomManager.Instance.Add(setting);

            roomObj.RoomId = room.RoomId;
            roomObj.Info.Transform.Pos = session.MyPlayer.Info.Transform.Pos;
            _roomObjs.Add(roomObj.Id, roomObj);

            EnterGame(roomObj);
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
