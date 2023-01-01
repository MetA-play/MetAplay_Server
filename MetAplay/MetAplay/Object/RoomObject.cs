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
        public RoomObject() : base()
        {
            ObjectType = GameObjectType.Room;
        }
        public GameRoom Room { get; set; }

        public override TransformInfo Transform { get => base.Transform; set 
            { Console.WriteLine("Transform Chagne"); base.Transform = value; } }

    }
}
