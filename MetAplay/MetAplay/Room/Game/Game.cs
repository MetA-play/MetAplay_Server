using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public interface IGame
    {
        public void Init();
        public void Start();
        public void Update();
        public void End();
    }

    public enum GameState
    {
        Ready,
        Start,
        End,
    }

    public class Game : IGame
    {
        public GameState State { get; protected set; }

        public virtual void Init()
        {
            State = GameState.Ready;
        }

        public virtual void Start()
        {
            State = GameState.Start;
        }

        public virtual void Update()
        {

        }

        public virtual void End()
        {
            State = GameState.End;
        }
    }
}