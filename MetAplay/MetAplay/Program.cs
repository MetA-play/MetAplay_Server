using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using ServerCore;

namespace MetAplay
{
    class Program
    {
        static Listener _listener = new Listener();
        static List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();
        /*static void TickRoom(GameRoom room, int tick = 100)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = tick;
            timer.Elapsed += ((s, e) => { room.Update(); });
            timer.AutoReset = true;
            timer.Enabled = true;

            _timers.Add(timer);
        }*/

        static void Main(string[] args)
        {

            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");

            //SoccerBall ball = new SoccerBall();
            //ball.Transform.Pos.X = 2;
            //ball.Transform.Pos.Y = 0;
            //ball.Transform.Pos.Z = 2;
            //Lobby.Instance.Push(Lobby.Instance.EnterGame, ball);

            //FlushRoom();
            //JobTimer.Instance.Push(FlushRoom);
            // TODO 
            while (true)
            {
                //테스트용
                Lobby.Instance.Update();
                for (int i = 0; i < 5; i++)
                {
                    if (RoomManager.Instance.Find(i) != null)
                        RoomManager.Instance.Find(i).Update();
                }
            }
        }
    }
}
