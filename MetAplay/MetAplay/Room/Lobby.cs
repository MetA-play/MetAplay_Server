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
        public Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public override void EnterGame(GameObject gameObject)
        {
            S_EnterGame enterGame = new S_EnterGame();


        }
    }
}
