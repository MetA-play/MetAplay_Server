using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class SoccerBall :GameObject
    {
        public Vector AddedForce { get; set; } = new Vector();
        
        
        long _nextMoveTick = 0;

        public SoccerBall()
        {
            ObjectType = GameObjectType.SoccerBall;
            speed = 8;
        }
        public override void Update()
        {
            MoveUpdate();
        }
        public virtual void MoveUpdate()
        {
            if (_nextMoveTick > Environment.TickCount64)
                return;

            int moveTick = (int)(1000 / speed);
            _nextMoveTick = Environment.TickCount64 + moveTick; // 이동속도와 연관


            Move();

            S_Move move = new S_Move();
            move.Id = Id;
            move.Transform = Transform;
            Lobby.Instance.Broadcast(move);
        }

        public void Move()
        {

            AddedForce.X = Lerp(AddedForce.X, 0, .3f);
            AddedForce.Y = Lerp(AddedForce.Y, 0, .3f);
            AddedForce.Z = Lerp(AddedForce.Z, 0, .3f);


            Transform.Pos.X += AddedForce.X;
            Transform.Pos.Y += AddedForce.Y;
            Transform.Pos.Z += AddedForce.Z;
        }

        public void KickIt(Vector hitterPos)
        {
            Vector forceVec = new Vector();
         
            Vector dir = new Vector();
            dir.X = Transform.Pos.X - hitterPos.X;
            dir.Y = Transform.Pos.X - hitterPos.Y;
            dir.Z = Transform.Pos.X - hitterPos.Z;

            float magnitude = MathF.Sqrt(dir.X * dir.X + dir.Z * dir.Z);


            dir.X = dir.X / magnitude;
            dir.Z = dir.Z / magnitude;

            forceVec = new Vector() { X = dir.X * 2 * 0.05f, Y = dir.Y * 2 * 0.05f, Z = dir.Z * 2 * 0.05f };

            AddedForce.X += forceVec.X;
            AddedForce.Y += forceVec.Y;
            AddedForce.Z += forceVec.Z;
        }

        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }
    }
}
