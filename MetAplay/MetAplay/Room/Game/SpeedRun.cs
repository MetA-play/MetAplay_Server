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
        public override void Init(GameRoom room)
        {
            base.Init(room);


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
            player.GetSlow(3000);
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
