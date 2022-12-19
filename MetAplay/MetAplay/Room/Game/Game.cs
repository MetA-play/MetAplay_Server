using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public interface IGame
    {
        public void Init(GameRoom room);
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
        public GameRoom Room { get; set; }
        public GameState State { get; protected set; }

        public List<GameObject> _objects = new List<GameObject>();

        public virtual void Init(GameRoom room)
        {
            State = GameState.Ready;
            Room = room;
            _objects.Clear();
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