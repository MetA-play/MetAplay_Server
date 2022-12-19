﻿using Google.Protobuf.Protocol;

namespace MetAplay
{
    public interface IGame
    {
        public void Init(GameRoom room);
        public void Start();
        public void Update();
        public void End();
    }

    public class Game : IGame
    {
        public GameRoom Room { get; set; }
        public GameType GameName { get; } = GameType.AvoidLog;
        public GameState State { get; protected set; }

        public List<GameObject> _objects = new List<GameObject>();

        public virtual void Init(GameRoom room)
        {
            if (room == null) return;
            Room = room;
            State = GameState.Waiting;
        }

        public virtual void Start()
        {
            if (Room == null || State != GameState.Waiting) return;
            State = GameState.Playing;

            // Init에서 등록된 오브젝트가 있다면 스폰
            if (_objects.Count > 0)
            {
                S_Spawn spawnPacket = new S_Spawn();
                foreach (GameObject go in _objects)
                    spawnPacket.Objects.Add(go.Info);
                Room.Broadcast(spawnPacket);
            }
        }

        public virtual void Update()
        {
            foreach (GameObject obj in _objects)
            {
                obj.Update();
            }
        }

        public virtual void End()
        {
            State = GameState.Ending;
        }
    }
}
