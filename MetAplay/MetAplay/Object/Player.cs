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

        public int speed = 5;
        public int inputFlag;
        public ClientSession Session { get; set; }
        public GameRoom Room { get; set; }
        public Player() : base()
        {
            ObjectType = GameObjectType.Player;
        }

        public TransformInfo SpawnPoint { get; set; }

        public UserInfo UserData { get { return Session.UserData; } set { Session.UserData = value; } }
        public bool IsDead { get; set; }

        public override void Update()
        {
            base.Update();

            MoveUpdate();
        }

        long _nextMoveTick = 0;

        public virtual void MoveUpdate()
        {
            if (_nextMoveTick > Environment.TickCount64)
                return;

            int moveTick = (int)(1000 / speed);
            _nextMoveTick = Environment.TickCount64 + moveTick; // 이동속도와 연관


            Move();
        }
        public virtual void Move()
        {
            float x = ((inputFlag >> 27) == 1) ? 1 : ((inputFlag >> 27) == 2) ? -1 : 0;
            float z = ((inputFlag >> 23 & 0b1111) == 1) ? 1 : ((inputFlag >> 23 & 0b1111) == 2) ? -1 : 0;
            //Console.WriteLine($"x: {x}   z:{z}");
            
            Vector dirVec = new Vector();
            float magnitude = MathF.Sqrt(x * x + z * z);
            //Console.WriteLine($"mag: {magnitude}");
            dirVec.X = x / magnitude;
            dirVec.Z = z / magnitude;

            if(x == 0 && z == 0)
            {
                dirVec.X = 0;
                dirVec.Z = 0;
            }

            Transform.Pos.X += dirVec.X;
            Transform.Pos.Z += dirVec.Z;
            //Console.WriteLine($"{dirVec.X},           {dirVec.Z}");
            //Console.WriteLine($"{Id}:   {Transform.Pos.X}, {Transform.Pos.Y}, {Transform.Pos.Z}");
        }
    }
}
