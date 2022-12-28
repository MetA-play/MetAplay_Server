using Google.Protobuf;
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
    public static void C_CreateRoomReqHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_CreateRoomReq req = packet as C_CreateRoomReq;

        Lobby.Instance.Push(Lobby.Instance.CreateRoomHandle,clientSession.MyPlayer,req.Setting);
    }

    public static void C_JoinRoomReqHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_JoinRoomReq req = packet as C_JoinRoomReq;
        Console.WriteLine("Join Try");
        Lobby.Instance.Push(Lobby.Instance.JoinRoomHandle, req.RoomId,clientSession.MyPlayer);

    }

    public static void C_MoveHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;

    }

    public static void C_ChatHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_Chat chat = packet as C_Chat;

        GameRoom room = clientSession.MyPlayer.Room;
        if (room == null)
            Lobby.Instance.Push(Lobby.Instance.ChatHandle, clientSession.MyPlayer, chat);
        else
            room.Push(room.ChatHandle, clientSession.MyPlayer, chat);
    }

    public static void C_UpdateGameStateReqHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;
        
    }

    public static void C_SetUserinfoHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_SetUserinfo info = packet as C_SetUserinfo;

        clientSession.UserData = info.Info;
    }
}
