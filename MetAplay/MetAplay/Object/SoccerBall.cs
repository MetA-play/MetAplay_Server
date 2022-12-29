using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class SoccerBall : GameObject
    {
        public Vector AddedForce { get; set; } = new Vector();


        long _nextMoveTick = 0;

        public SoccerBall()
        {
            ObjectType = GameObjectType.SoccerBall;
            speed = 100;
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

            S_SyncPos move = new S_SyncPos();
            move.Id = Id;
            move.Transform = Transform;
            Lobby.Instance.Broadcast(move);
        }

        public void Move()
        {
            AddedForce.X = Lerp(AddedForce.X, 0, .1f);
            AddedForce.Y = Lerp(AddedForce.Y, 0, .1f);
            AddedForce.Z = Lerp(AddedForce.Z, 0, .1f);

            if (AddedForce.X < 0.05f)
            {
                AddedForce.X = 0;
            }
            else if (AddedForce.Y < 0.05f)
            {
                AddedForce.Y = 0;
            }
            else if (AddedForce.Z < 0.05f)
            {
                AddedForce.Z = 0;
            }

            Transform.Pos.X += AddedForce.X;
            Transform.Pos.Z += AddedForce.Z;
        }

        public void KickIt(Vector hitterPos)
        {
            Vector forceVec = new Vector();
            //Console.WriteLine($"hitPos: {hitterPos.X}   {hitterPos.Z}");
            Vector dir = new Vector();
            dir.X = Transform.Pos.X - hitterPos.X;
            dir.Z = Transform.Pos.Z - hitterPos.Z;
            //Console.WriteLine($"My Pos: {Transform.Pos.X}    {Transform.Pos.Z}");
            //Console.WriteLine($"Hit Pos: {hitterPos.X}    {hitterPos.Z}");
            float magnitude = MathF.Sqrt(dir.X * dir.X + dir.Z * dir.Z);
            dir.X = dir.X / magnitude;
            dir.Z = dir.Z / magnitude;
            //Console.WriteLine($"Dir:  {dir.X}     {dir.Z}");

            forceVec = new Vector() { X = dir.X * 20 * 0.05f, Y = dir.Y * 20 * 0.05f, Z = dir.Z * 20 * 0.05f };
            //Console.WriteLine($"ForceVec: {forceVec.X}        {forceVec.Z}");
            AddedForce.X += forceVec.X;
            AddedForce.Z += forceVec.Z;
        }

        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }
    }
}
