﻿using Google.Protobuf;
using Google.Protobuf.Protocol;
using MetAplay;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PacketHandler
{

    public static void C_CreateroomReqHandler(PacketSession session, IMessage packet)
    {
        ClientSession CS = session as ClientSession;
        C_CreateroomReq req = packet as C_CreateroomReq;
        
    }
    public static void C_JoinroomReqHandler(PacketSession session, IMessage packet)
    {
        ClientSession CS = session as ClientSession;

    }
    public static void C_MoveHandler(PacketSession session, IMessage packet)
    {
        ClientSession CS = session as ClientSession;

    }
}
