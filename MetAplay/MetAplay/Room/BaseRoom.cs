using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetAplay
{
    public class BaseRoom : JobSerializer
    {
        public int RoomId { get; set; }

        protected Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public virtual void Update()
        {
            foreach (Player p in _players.Values)
                p.Update();
            
            
            Flush();
        }

        public virtual void EnterGame(GameObject gameObject)
        {

        }

        public virtual void LeaveGame(int gameObjectId)
        {

        }

        public void MoveHandle(GameObject gameObject, C_Move movePacket)
        {
            if (gameObject == null) return; 

            gameObject.Info.State = movePacket.State;

            if(gameObject.ObjectType == GameObjectType.Player)
            {
                Player player = gameObject as Player;
                player.inputFlag = movePacket.InputFlag;
            }


            S_Move resMovePacket = new S_Move();
            resMovePacket.Id = gameObject.Id;
            resMovePacket.Transform = movePacket.Transform;
            resMovePacket.State = movePacket.State;
            resMovePacket.InputFlag = movePacket.InputFlag;

            Broadcast(resMovePacket);
        }

        public void MoveHandle(GameObject gameObject, C_SyncPos movePacket)
        {
            gameObject.Transform = movePacket.Transform;

            S_SyncPos res = new S_SyncPos();
            res.Id = gameObject.Id;
            res.Transform = movePacket.Transform;


            Broadcast(res);

        }

        public void ChatHandle(Player player, C_Chat chat)
        {
            S_Chat res = new S_Chat();
            res.PlayerId = player.Id;
            res.Content = chat.Content;
            Broadcast(res);
        }

        public void Broadcast(IMessage packet)
        {
            foreach (Player p in _players.Values)
                p.Session.Send(packet);
        }

        public Player FindPlayerById(int playerId)
        {
            Player player = null;

            foreach (var p in _players.Values)
            {
                if (p.Id == playerId)
                    player = p;
            }

            return player;
        }
    }
}