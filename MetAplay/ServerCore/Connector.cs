﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Connector
    {
        Func<Session> _sessionCreateFunc;

        public void Connect(IPEndPoint endPoint, Func<Session> sessionCreateFunc, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _sessionCreateFunc = sessionCreateFunc;


                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += ConnectCompleted;
                args.RemoteEndPoint = endPoint;
                args.UserToken = socket;

                RegisterConnect(args);
            }
        }

        void RegisterConnect(SocketAsyncEventArgs args)
        {
            Socket socket = args.UserToken as Socket;

            if (socket == null)
                return;

            bool pending = socket.ConnectAsync(args);
            if (pending == false)
                ConnectCompleted(null, args);
        }

        void ConnectCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = _sessionCreateFunc.Invoke();
                session.Start(args.ConnectSocket);
                session.OnConnected(args.RemoteEndPoint);
            }
            else
            {
                Console.WriteLine($"ConnectCompleted Fail:{args.SocketError}");
            }
        }
    }
}
