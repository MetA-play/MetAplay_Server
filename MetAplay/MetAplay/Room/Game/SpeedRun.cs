using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class SpeedRun : Game
    {
        private void SetSpawnPoint()
        {
            SpawnPoints = new List<TransformInfo>()
            {
                new TransformInfo() { Pos = new Vector() { X = -6, Y = 3, Z = 18 } },
                new TransformInfo() { Pos = new Vector() { X = -2, Y = 3, Z = 18 } },
                new TransformInfo() { Pos = new Vector() { X = 2, Y = 3, Z = 18 } },
                new TransformInfo() { Pos = new Vector() { X = 6, Y = 3, Z = 18 } },
                new TransformInfo() { Pos = new Vector() { X = -6, Y = 3, Z = 14 } },
                new TransformInfo() { Pos = new Vector() { X = -2, Y = 3, Z = 14 } },
                new TransformInfo() { Pos = new Vector() { X = 2, Y = 3, Z = 14 } },
                new TransformInfo() { Pos = new Vector() { X = 6, Y = 3, Z = 14 } }
            };
        }

        private TransformInfo[] GameSpawnPoints = new TransformInfo[]
        {
            new TransformInfo() { Pos = new Vector() { X = 0, Y = 3, Z = 11 } },
            new TransformInfo() { Pos = new Vector() { X = 40, Y = 3, Z = 95 } },
            new TransformInfo() { Pos = new Vector() { X = 130, Y = 3, Z = 95 } },
            new TransformInfo() { Pos = new Vector() { X = 153, Y = 3, Z = 165 } },
            new TransformInfo() { Pos = new Vector() { X = 120, Y = 3, Z = 247 } },
            new TransformInfo() { Pos = new Vector() { X = 34, Y = 3, Z = 247 } },
            new TransformInfo() { Pos = new Vector() { X = -32, Y = 3, Z = 295 } },
            new TransformInfo() { Pos = new Vector() { X = -21, Y = 3, Z = 348 } },
            new TransformInfo() { Pos = new Vector() { X = 46, Y = 3, Z = 355 } },
            new TransformInfo() { Pos = new Vector() { X = 68, Y = 3, Z = 436 } }
        };

        public override void Init(GameRoom room)
        {
            base.Init(room);
            GameName = GameType.SpeedRun;
            SetSpawnPoint();
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
            S_SyncPos syncPosPacket = new S_SyncPos();
            syncPosPacket.Id = playerId;
            syncPosPacket.Transform = Room.FindPlayerById(playerId).SpawnPoint;
            Room.Broadcast(syncPosPacket);
        }

        public void OnTouchEndLine(int playerId)
        {
            S_GameEnd endGamePacket = new S_GameEnd();
            endGamePacket.WinnerId = playerId;
            Room.Broadcast(endGamePacket);
            UpdateGameState(GameState.Ending);
        }
    }
}
