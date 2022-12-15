using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class AvoidLog : Game
    {
        private GameObject _log;
        private float LogRotY
        {
            get { return _log.Info.Transform.Rot.Y; }
            set { _log.Info.Transform.Rot.Y = value; }
        }

        private int _speedUpTick = 15000;

        public override void Init()
        {
            base.Init();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            
            _log.Update();
        }

        public override void End()
        {
            base.End();
        }

        async private void SpeedUp()
        {
            if (!State.Equals(GameState.Start)) return;
            await Task.Delay(_speedUpTick);
            // TODO
        }
    }
}
