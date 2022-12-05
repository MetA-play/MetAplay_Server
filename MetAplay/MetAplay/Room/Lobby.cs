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

        public override void EnterGame(GameObject gameObject)
        {

            if(gameObject.ObjectType == GameObjectType.Player)
            {
                Player player = gameObject as Player;
                _players.Add(gameObject.Id,player);

                {
                    S_EnterGame entergame = new S_EnterGame();
                    player.Session.Send(entergame);
                    S_Spawn spawn = new S_Spawn();

                    foreach (Player p in _players.Values)
                    {
                        if(player != p)
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
    }
}
