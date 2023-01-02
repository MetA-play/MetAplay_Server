using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;

namespace MetAplay
{
    class Program
    {
        static Listener _listener = new Listener();
        static List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();

        public static void TickRoom(GameRoom room, int tick = 100)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = tick;
            timer.Elapsed += ((s, e) => { room.Update(); });
            timer.AutoReset = true;
            timer.Enabled = true;

            _timers.Add(timer);
        }

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = IPAddress.Parse("192.168.214.234");
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");

            SoccerBall ball = ObjectManager.Instance.Add<SoccerBall>();
            ball.Transform.Pos.X = 15;
            ball.Transform.Pos.Y = 2.49f;
            ball.Transform.Pos.Z =140;
            Lobby.Instance.Push(Lobby.Instance.EnterGame, ball);

            //FlushRoom();
            //JobTimer.Instance.Push(FlushRoom);
            // TODO 
            while (true)
            {
                Lobby.Instance.Update();
                Thread.Sleep(100);
            }
        }
    }
}
