using Google.Protobuf.Protocol;
using MetAplay.Object;
using Server;
using Server.Game;
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

        public override void EnterGame(GameObject gameObject)
        {
            if (gameObject == null) return;

            if (gameObject.ObjectType == GameObjectType.Player)
            {
                Player player = gameObject as Player;
                _players.Add(player.Id, player);

                {
                    S_EnterGame enterGame = new S_EnterGame();
                    player.Session.Send(enterGame);

                }
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    foreach (Player p in _players.Values)
                    {
                        if (player != p)
                            spawnPacket.Objects.Add(p.Info);
                    }
                    player.Session.Send(spawnPacket);
                }
            }

            {
                S_Spawn spawnPacket = new S_Spawn();
                spawnPacket.Objects.Add(gameObject.Info);
            }
        }

        public override void LeaveGame(int gameObjectId)
        {

        }
    }
}
