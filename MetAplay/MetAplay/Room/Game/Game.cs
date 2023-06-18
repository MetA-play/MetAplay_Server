using Google.Protobuf.Protocol;

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
        public GameType GameName { get; protected set; }
        public GameState State { get; protected set; }
        public List<GameObject> Objects = new List<GameObject>();
        public List<TransformInfo> SpawnPoints = new List<TransformInfo>();

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
            if (Objects.Count > 0)
            {
                S_Spawn spawnPacket = new S_Spawn();
                foreach (GameObject go in Objects)
                    spawnPacket.Objects.Add(go.Info);
                Room.Broadcast(spawnPacket);
            }

            S_UpdateGameStateRes res = new S_UpdateGameStateRes();
            res.State = State;
            Room.Broadcast(res);

            int index = 0;
            foreach (Player player in Room.Players)
            {
                player.Transform = SpawnPoints[index++];
                S_SyncPos syncPosPacket = new S_SyncPos();
                syncPosPacket.Id = player.Id;
                syncPosPacket.Transform = player.Transform;
                Room.Broadcast(syncPosPacket);
            }
        }

        public virtual void Update()
        {
            foreach (GameObject obj in Objects)
                obj.Update();
        }

        public virtual void End()
        {
            if (Room == null || State != GameState.Playing) return;
            State = GameState.Ending;
            Console.WriteLine("Game End");
            Room.PushAfter(5000, Room.BackLobby);
        }

        public void UpdateGameState(GameState state)
        {   
            if (State == GameState.Ending) return;

            if (state == GameState.Playing)
                Start();

            if (state == GameState.Ending)
                End();
        }
        
        public void PlayerDead(int playerId)
        {
            Player player = Room.FindPlayerById(playerId);
            if (player == null) return;
            player.IsDead = true;

            S_PlayerDead deadPacket = new S_PlayerDead();
            deadPacket.PlayerId = player.Id;
            Room.Broadcast(deadPacket);

            GameOverCheck();
        }

        public void GameOverCheck()
        {
            int leavePlayerCount = Room.Players.Count(p => p.IsDead == false);

            if (leavePlayerCount == 1)
            {
                if (State == GameState.Playing)
                {
                    UpdateGameState(GameState.Ending);
                }
            }
        }
    }
}
