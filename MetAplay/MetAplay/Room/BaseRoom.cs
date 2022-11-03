using MetAplay.Object;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class BaseRoom : JobSerializer
    {
        public int RoomId { get; set; }

        Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public virtual void EnterGame(GameObject gameObject)
        {

        }
        public virtual void LeaveGame(int gameObjectId)
        {

        }
    }
}
