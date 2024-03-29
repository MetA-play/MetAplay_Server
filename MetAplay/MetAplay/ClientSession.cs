﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using System.Net;
using Google.Protobuf.Protocol;
using Google.Protobuf;
using System.Runtime.Serialization;
using MetAplay;

namespace MetAplay
{
    public class ClientSession : PacketSession
    {

        public UserInfo UserData { get; set; }
        Player myPlayer;
        public Player MyPlayer 
        { get 
            { 
                return myPlayer;
            } 
            set 
            {
                if(value == null)
                    Console.WriteLine("null insert");
                Console.WriteLine("Player Change");
                myPlayer = value ; 
            }
        }
        public int SessionId { get; set; }

        public void Send(IMessage packet)
        {
            string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);
            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
            Send(new ArraySegment<byte>(sendBuffer));
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
            //Player player = ObjectManager.Instance.Add<Player>();
            //player.Session = this;
            //MyPlayer = player;
            //player.Info.Transform.Pos.X = 15;
            //player.Info.Transform.Pos.Y = 0;
            //player.Info.Transform.Pos.Z = 118;
            //Lobby.Instance.Push(Lobby.Instance.EnterGame, player);
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            if(myPlayer.Room == null)
                Lobby.Instance.Push(Lobby.Instance.LeaveGame, MyPlayer.Info.Id);
            else
                myPlayer.Room.Push(myPlayer.Room.LeaveGame, myPlayer.Info.Id);

            SessionManager.Instance.Remove(this);

            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
    