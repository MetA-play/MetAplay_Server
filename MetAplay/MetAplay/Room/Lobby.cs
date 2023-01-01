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
        Dictionary<int, SoccerBall> _soccerBalls = new Dictionary<int, SoccerBall>();

        public override void Update()
        {
            foreach (SoccerBall ball in _soccerBalls.Values)
                ball.Update();
            base.Update();

        }
        public void CreateRoomHandle(Player player, RoomSetting setting)
        {
            RoomObject roomObj = ObjectManager.Instance.Add<RoomObject>();
            GameRoom room = RoomManager.Instance.Add(setting);
            roomObj.Room = room;
            roomObj.Info.Transform.Pos = player.Info.Transform.Pos;
            roomObj.Transform.Pos.Y += 2.5f;
            _roomObjs.Add(roomObj.Id, roomObj);
            EnterGame(roomObj);

            S_CreateRoomRes res = new S_CreateRoomRes();
            res.Info = new RoomInfo();
            res.Info.Setting = setting;
            res.Info.Id = room.RoomId;
            res.ObjectId = roomObj.Id;
            player.Session.Send(res);
            LeaveGame(player.Id);
        }

        public void JoinRoomHandle(int roomId, Player player)
        {
            GameRoom room = RoomManager.Instance.Find(roomId);

            if (room == null)
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
            res.Info = new RoomInfo();
            res.Info.Id = roomId;
            player.Session.Send(res);

            player.Transform.Pos.X = 0;
            player.Transform.Pos.Y = 0;
            player.Transform.Pos.Z = 0;
            room.EnterGame(player);
        }

        public void DeleteRoom()
        {

        }

        public override void EnterGame(GameObject gameObject)
        {
            if (gameObject.ObjectType == GameObjectType.Player)
            {
                Player player = gameObject as Player;
                player.Info.UserData = player.Session.UserData;
                _players.Add(gameObject.Id, player);

                player.Session.MyPlayer = player;

                {
                    {
                        S_EnterGame enterGamePacket = new S_EnterGame();
                        enterGamePacket.Player = player.Info;
                        player.Session.Send(enterGamePacket);

                        S_Spawn spawn = new S_Spawn();
                        foreach (Player p in _players.Values)
                        {
                            if (p != player)
                                spawn.Objects.Add(p.Info);

                        }
                        foreach (RoomObject obj in _roomObjs.Values)
                            spawn.Objects.Add(obj.Info);
                        foreach (SoccerBall ball in _soccerBalls.Values)
                            spawn.Objects.Add(ball.Info);

                        player.Session.Send(spawn);
                    }
                }

               
            }
            else if (gameObject.ObjectType == GameObjectType.SoccerBall)
            {
                SoccerBall ball = gameObject as SoccerBall;
                _soccerBalls.Add(ball.Id, ball);
            }

            {


                S_Spawn spawn = new S_Spawn();

                if (gameObject.ObjectType == GameObjectType.Room)
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

        public void SoccerballHandle(C_HitSoccerball hitBall)
        {
            SoccerBall ball = null;

            if (_soccerBalls.TryGetValue(hitBall.ObjectId, out ball) == false)
                return;

            ball.KickIt(hitBall.HitterTransform.Pos);
            
        }
    }
}
