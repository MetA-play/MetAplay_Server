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

        public int AddedRotY;

        public override void Update()
        {
            if (JoinedRoom == null) return;
            if (JoinedRoom.Content.State != GameState.Playing) return;
            
            int rotY = LogRotY + AddedRotY;
            LogRotY = rotY >= 360 ? rotY % 360 : rotY;

            S_Move movePacket = new S_Move();
            movePacket.Id = Id;
            movePacket.Transform = Info.Transform;
            movePacket.State = Info.State;
            JoinedRoom.Broadcast(movePacket);
        }
    }
    #endregion

    public class AvoidLog : Game
    {   
        public override void Init(GameRoom room)
        {
            base.Init(room);
            GameName = GameType.AvoidLog;
            
            // 통나무 생성
            Log log1 = ObjectManager.Instance.Add<Log>();
            _objects.Add(log1);
            log1.JoinedRoom = Room;
            log1.AddedRotY = 15;
            log1.Info.PrefabName = "Log1";
            log1.SetPosition(0f, 8f, 0f);

            //Log log2 = ObjectManager.Instance.Add<Log>();
            //_objects.Add(log2);
            //log2.JoinedRoom = Room;
            //log2.AddedRotY = 10;
            //log2.Info.PrefabName = "Log2";
            //log2.SetPosition(0f, 1.9f, 0f);
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void End()
        {
            base.End();
        }
    }
}
