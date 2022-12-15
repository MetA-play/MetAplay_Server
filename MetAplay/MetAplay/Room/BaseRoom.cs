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
            Flush();
        }
        public virtual void EnterGame(GameObject gameObject)
        {

        }
        public virtual void LeaveGame(int gameObjectId)
        {

        }

        public void MoveHandle(Player player, C_Move movePacket)
        {
            if (player == null) return;

            player.Info.Transform = movePacket.Transform;
            player.Info.State = movePacket.State;

            S_Move resMovePacket = new S_Move();
            resMovePacket.Id = player.Id;
            resMovePacket.Transform = movePacket.Transform;
            resMovePacket.State = movePacket.State;

            Broadcast(resMovePacket);
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
            {
                p.Session.Send(packet);
            }
        }
    }
}