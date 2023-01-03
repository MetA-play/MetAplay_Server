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

        public override void Init(GameRoom room)
        {
            base.Init(room);
            GameName = GameType.SpeedRun;
            SetSpawnPoint();
        }

        public override void Start()
        {
            base.Start();
            
            foreach (Player player in Room.Players)
            {
                player.SpawnPoint = new TransformInfo()
                {
                    Pos = new Vector() { X = 0, Y = 3, Z = 11 }
                };
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void End()
        {
            base.End();
        }

        public void CollideObstacle(Player player)
        {
            Console.WriteLine("CollideObstacle");
            S_SyncPos syncPosPacket = new S_SyncPos();
            syncPosPacket.Id = player.Id;
            syncPosPacket.Transform = player.SpawnPoint;
            Room.Broadcast(syncPosPacket);
        }

        public void OnTouchEndLine(int playerId)
        {
            if (State == GameState.Ending) return;
            S_GameEnd endGamePacket = new S_GameEnd();
            endGamePacket.WinnerId = playerId;
            Room.Broadcast(endGamePacket);
            UpdateGameState(GameState.Ending);
        }
    }
}
