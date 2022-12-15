using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class RoomObject : GameObject
    {

        public GameRoom Room { get; set; }
        
        public void Enter(Player player,bool isHost=false)
        {
            Room.EnterGame(player);


        }

        public void Leave()
        {

        }
    
    }
}
