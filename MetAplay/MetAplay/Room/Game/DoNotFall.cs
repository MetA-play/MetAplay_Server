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
        public bool[,] IsDeleted = new bool[3, 57];

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

        public override void Init(GameRoom room)
        {
            base.Init(room);
            GameName = GameType.DoNotFall;

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
            base.End();
        }

        public void DeleteFloorBlock(int floorIndex, int blockIndex)
        {
            
        }
    }
}
