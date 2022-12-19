﻿using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class Player : GameObject
    {
        public ClientSession Session { get; set; }
        public GameRoom Room { get; set; }
        public Player()
        {
            ObjectType = GameObjectType.Player;

        }
    }
}
