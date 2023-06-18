using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class GameRoom : BaseRoom
    {

        public GameRoom()
        {
            Content = new Game();
        }
        public RoomInfo Info
        {
            get { return new RoomInfo() { Id = RoomId, CurrentPersonnel = _players.Count, Setting = Setting }; }
        }
        public RoomSetting Setting { get; set; }
        public Player Host { get; set; }
        public Game Content { get; set; }
        public bool IsStart { get { return Content.State == GameState.Playing; } }

        public void Init()
        {
            switch (Setting.GameType)
            {
                case GameType.AvoidLog:
                    Content = new AvoidLog();
                    break;
                case GameType.DoNotFall:
                    Content = new DoNotFall();
                    break;
                case GameType.SpeedRun:
                    Content = new SpeedRun();
                    break;
            }

            Content.Init(this);
            PushAfter(1000, SendRoomInfo);
        }

        private void SendRoomInfo()
        {
            S_JoinRoomRes joinRoomRes = new S_JoinRoomRes();
            joinRoomRes.Info = Info;
            Broadcast(joinRoomRes);
            PushAfter(1000, SendRoomInfo);
        }

        public List<Player> Players { get
            {
                return _players.Values.ToList();
            }
        }

        public override void Update()
        {
            Content.Update();
            base.Update();
        }

        public override void EnterGame(GameObject gameObject)
        {
            if (gameObject == null) return;

            GameObjectType type = ObjectManager.GetObjectTypeById(gameObject.Id);

            if (type.Equals(GameObjectType.Player))
            {
                Player player = gameObject as Player;
                player.Info.UserData = player.Session.UserData;
                _players.Add(player.Id, player);
                player.Room = this;

                {
                    S_EnterGame enterGamePacket = new S_EnterGame();
                    enterGamePacket.Player = player.Info;
                    enterGamePacket.Player.UserData = player.UserData;
                    player.Session.Send(enterGamePacket);

                    S_Spawn spawnPacket = new S_Spawn();

                    foreach (Player p in _players.Values)
                        if (p != player) spawnPacket.Objects.Add(p.Info);

                    player.Session.Send(spawnPacket);
                }
            }
            else if (type.Equals(GameObjectType.None))
            {

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
            if (!_players.Remove(gameObjectId, out Player player)) return;

            player.Room = null;

            {
                S_LeaveGame leaveGamePacket = new S_LeaveGame();
                player.Session.Send(leaveGamePacket);
            }

            {
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.ObjectId.Add(gameObjectId);
                foreach (Player p in _players.Values)
                    if (p.Id != player.Id) p.Session.Send(despawnPacket);
            }
        }

        public void BackLobby()
        {
            foreach (Player player in Players)
            {
                LeaveGame(player.Id);
            }

            RoomManager.Instance.Remove(RoomId);
        }
    }
}
