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
        public override void EnterGame(GameObject gameObject)
        {
            if (gameObject is null) return;
            if (gameObject is not Player player) return;

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

        public void HandleMove(Player player, C_Move movePacket)
        {
            if (player is null) return;

            player.Info.Transform = movePacket.Transform;

            S_Move resMovePacket = new S_Move();
            resMovePacket.Id = player.Id;
            resMovePacket.Transform = movePacket.Transform;

            Broadcast(resMovePacket);
        }
    }
}
