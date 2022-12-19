using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    #region GameObject

    public class FloorBlock : GameObject
    {

    }

    #endregion

    public class DoNotFall : Game
    {


        public override void Init(GameRoom room)
        {
            base.Init(room);
            GameName = GameType.DoNotFall;


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
