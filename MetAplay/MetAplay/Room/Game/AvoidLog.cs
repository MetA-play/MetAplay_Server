using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    #region GameObject
    public class Log : GameObject
    {
        private int LogRotY
        {
            get { return (int)Info.Transform.Rot.Y; }
            set { Info.Transform.Rot.Y = value; }
        }

        private int _addedRotY = 15;

        public override void Update()
        {
            int rotY = LogRotY + _addedRotY;
            LogRotY = rotY > 360 ? rotY % 360 : rotY;

            if (Room == null) return;
            S_Move movePacket = new S_Move();
            movePacket.Id = Id;
            movePacket.Transform = Info.Transform;
            movePacket.State = Info.State;
            Room.Broadcast(movePacket);
        }
    }
    #endregion

    public class AvoidLog : Game
    {
        private Log log;
        
        public override void Init(GameRoom room)
        {
            base.Init(room);
            
        }
        public override void Start()
        {
            base.Start();

            if (Room == null) return;

            log.Room = Room;
            S_Spawn spawnPacket = new S_Spawn();
            foreach (GameObject go in _objects)
                spawnPacket.Objects.Add(go.Info);
            Room.Broadcast(spawnPacket);
        }
        public override void Update()
        {
            

            log.Update();
        }
        public override void End()
        {
            base.End();
        }
    }
}
