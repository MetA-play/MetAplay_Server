using Google.Protobuf.Protocol;
using MetAplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class GameRoom : BaseRoom
    {
        public RoomInfo Info
        {
            get { return new RoomInfo() { Id = RoomId, CurrentPersonnel = _players.Count, Setting = Setting }; }
        }
        public RoomSetting Setting { get; set; }
        public Player Host { get; set; }
        public Game Content { get; set; }
        public bool IsStart { get { return Content.State == GameState.Start; } }

        public override void Update()
        {
            base.Update();
            Content.Update();
        }
        public override void EnterGame(GameObject gameObject)
        {
            if (gameObject == null) return;

            GameObjectType type = ObjectManager.GetObjectTypeById(gameObject.Id);

            if (type.Equals(GameObjectType.Player))
            {
                Player player = gameObject as Player;
                _players.Add(player.Id, player);
                player.Room = this;

                // 본인에게 전송
                {
                    S_EnterGame enterGamePacket = new S_EnterGame();
                    enterGamePacket.Player = player.Info;
                    player.Session.Send(enterGamePacket);

                    S_Spawn spawnPacket = new S_Spawn();
                    foreach (Player p in _players.Values)
                        if (p != player)    
                            spawnPacket.Objects.Add(p.Info);
                    player.Session.Send(spawnPacket);
                }

                // 타인에게 전송
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    foreach (Player p in _players.Values)
                        if (p.Id != player.Id) p.Session.Send(spawnPacket);
                }
            }
            else if (type.Equals(GameObjectType.None))
            {

            }
        }

        public override void LeaveGame(int gameObjectId)
        {
            if (!_players.Remove(gameObjectId, out Player player)) return;

            player.Room = null;

            // 본인에게 전송
            {
                S_LeaveGame leaveGamePacket = new S_LeaveGame();
                player.Session.Send(leaveGamePacket);
            }

            // 타인에게 전송
            {
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.ObjectId.Add(gameObjectId);
                foreach (Player p in _players.Values)
                    if (p.Id != player.Id) p.Session.Send(despawnPacket);
            }
        }
    }
}