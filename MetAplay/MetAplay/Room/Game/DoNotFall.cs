using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MetAplay
{
    #region GameObjects
    
    public class FloorBlockController : GameObject
    {
        public bool[,] IsDeleted = new bool[3, 61];

        public void DeleteFloorBlock(int floorIndex, int blockIndex)
        {
            if (IsDeleted[floorIndex, blockIndex] == false)
            {
                IsDeleted[floorIndex, blockIndex] = true;

                S_DeleteFloorBlock deleteFloorBlockPacket = new S_DeleteFloorBlock();
                deleteFloorBlockPacket.FloorIndex = floorIndex;
                deleteFloorBlockPacket.BlockIndex = blockIndex;
                JoinedRoom.Broadcast(deleteFloorBlockPacket);
            }
        }
    }

    #endregion

    public class DoNotFall : Game
    {
        private FloorBlockController FBC;

        private void SetSpawnPoint()
        {
            SpawnPoints = new List<TransformInfo>()
            {
                new TransformInfo() { Pos = new Vector() { X = -14, Y = 66, Z = 0 } },
                new TransformInfo() { Pos = new Vector() { X = -10.5f, Y = 66, Z = -10 } },
                new TransformInfo() { Pos = new Vector() { X = 0, Y = 66, Z = -16 } },
                new TransformInfo() { Pos = new Vector() { X = 10.5f, Y = 66, Z = -10 } },
                new TransformInfo() { Pos = new Vector() { X = 14, Y = 66, Z = 0 } },
                new TransformInfo() { Pos = new Vector() { X = 10.5f, Y = 66, Z = 10 } },
                new TransformInfo() { Pos = new Vector() { X = 0, Y = 66, Z = 16 } },
                new TransformInfo() { Pos = new Vector() { X = -10.5f, Y = 66, Z = 10 } }
            };
        }

        public override void Init(GameRoom room)
        {
            base.Init(room);
            GameName = GameType.DoNotFall;
            SetSpawnPoint();

            FBC = ObjectManager.Instance.Add<FloorBlockController>();
            Objects.Add(FBC);
            FBC.JoinedRoom = Room;
            FBC.Info.PrefabName = "FloorBlocks";
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
            S_GameEnd endGamePacket = new S_GameEnd();
            endGamePacket.WinnerId = Room.Players.Find(p => p.IsDead == false).Id;
            Room.Broadcast(endGamePacket);
            Console.WriteLine($"게임 종료 Winner: {endGamePacket.WinnerId}");
            base.End();
        }

        public void DeleteFloorBlock(int floorIndex, int blockIndex)
        {
            if (floorIndex > 2 || blockIndex > 60) return;
            FBC.DeleteFloorBlock(floorIndex, blockIndex);
        }
    }
}
