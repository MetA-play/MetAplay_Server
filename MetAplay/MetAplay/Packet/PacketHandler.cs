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

    public static void C_CreateroomReqHandler(PacketSession session, IMessage packet)
    {
        ClientSession CS = session as ClientSession;
        C_CreateroomReq req = packet as C_CreateroomReq;

        Lobby.Instance.Push(Lobby.Instance.CreateRoomHandle,CS.MyPlayer,req.Setting);
    }
    public static void C_JoinroomReqHandler(PacketSession session, IMessage packet)
    {
        ClientSession CS = session as ClientSession;
        C_JoinroomReq req = packet as C_JoinroomReq;
        Console.WriteLine("Join Try");
        Lobby.Instance.Push(Lobby.Instance.JoinRoomHandle, req.RoomId,CS.MyPlayer);

    }
    public static void C_MoveHandler(PacketSession session, IMessage packet)
    {
        ClientSession CS = session as ClientSession;

    }

    public static void C_ChatHandler(PacketSession session, IMessage packet)
    {
        ClientSession CS = session as ClientSession;
        C_Chat chat = packet as C_Chat;

        GameRoom room = CS.MyPlayer.Room;
        if (room == null)
            Lobby.Instance.Push(Lobby.Instance.ChatHandle, CS.MyPlayer, chat);
        else
            room.Push(room.ChatHandle, CS.MyPlayer, chat);
        
    }
    public static void C_UpdateGameStateReqHandler(PacketSession session, IMessage packet)
    {
        ClientSession CS = session as ClientSession;
        
    }
}
