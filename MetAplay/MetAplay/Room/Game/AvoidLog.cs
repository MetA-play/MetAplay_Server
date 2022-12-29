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

            LogRotY += AddedRotY;

            S_Move movePacket = new S_Move();
            movePacket.Id = Id;
            movePacket.Transform = Info.Transform;
            movePacket.State = Info.State;
            JoinedRoom.Broadcast(movePacket);

            if (LogRotY == 360)
                LogRotY = 0;
        }
    }
    #endregion

    public class AvoidLog : Game
    {
        private void SetSpawnPoint()
        {
            SpawnPoints = new List<TransformInfo>()
            {
                new TransformInfo() { Pos = new Vector() { X = -9, Y = 2, Z = -8 } },
                new TransformInfo() { Pos = new Vector() { X = -3, Y = 2, Z = -8 } },
                new TransformInfo() { Pos = new Vector() { X = 3, Y = 2, Z = -8 } },
                new TransformInfo() { Pos = new Vector() { X = 9, Y = 2, Z = -8 } },
                new TransformInfo() { Pos = new Vector() { X = -9, Y = 2, Z = -14 } },
                new TransformInfo() { Pos = new Vector() { X = -3, Y = 2, Z = -14 } },
                new TransformInfo() { Pos = new Vector() { X = 3, Y = 2, Z = -14 } },
                new TransformInfo() { Pos = new Vector() { X = 9, Y = 2, Z = -14 } }
            };
        }

        public override void Init(GameRoom room)
        {
            base.Init(room);
            GameName = GameType.AvoidLog;
            SetSpawnPoint();
            
            // 통나무 생성
            Log log1 = ObjectManager.Instance.Add<Log>();
            Objects.Add(log1);
            log1.JoinedRoom = Room;
            log1.AddedRotY = 15;
            log1.Info.PrefabName = "Log1";
            log1.SetPosition(0f, 8f, 0f);

            Log log2 = ObjectManager.Instance.Add<Log>();
            Objects.Add(log2);
            log2.JoinedRoom = Room;
            log2.AddedRotY = 10;
            log2.Info.PrefabName = "Log2";
            log2.SetPosition(0f, 1.9f, 0f);
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

        public void CollideObstacle(int playerId)
        {
            Player player = Room.FindPlayerById(playerId);
            if (player == null) return;
            player.IsDead = true;

            S_PlayerDead deadPacket = new S_PlayerDead();
            deadPacket.PlayerId = player.Id;
            Room.Broadcast(deadPacket);

            GameOverCheck();
        }
    }
}
