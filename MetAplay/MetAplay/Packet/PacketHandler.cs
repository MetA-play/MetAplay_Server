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
        C_Move move = packet as C_Move;

        if (clientSession.MyPlayer.Room == null)
            Lobby.Instance.Push(Lobby.Instance.MoveHandle, clientSession.MyPlayer, move);
        else
            clientSession.MyPlayer.Room.Push(clientSession.MyPlayer.Room.MoveHandle, clientSession.MyPlayer, move);
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
        C_UpdateGameStateReq statePacket = packet as C_UpdateGameStateReq;

        if (clientSession.MyPlayer.Room.Content == null) return;
        clientSession.MyPlayer.Room.Push(clientSession.MyPlayer.Room.Content.UpdateGameState, statePacket.State);
    }

    public static void C_SetUserinfoHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_SetUserinfo info = packet as C_SetUserinfo;

        clientSession.UserData = info.Info;
    }

    public static void C_HitSoccerballHandler(PacketSession session, IMessage packet)
    {

    }

    public static void C_SyncPosHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_SyncPos sync = packet as C_SyncPos;

        if (clientSession.MyPlayer.Room != null)
        {
            clientSession.MyPlayer.Room.Push(clientSession.MyPlayer.Room.MoveHandle, clientSession.MyPlayer, sync);
        }
        else
        {
            Lobby.Instance.Push(Lobby.Instance.MoveHandle, clientSession.MyPlayer, sync);

        }
    }

    public static void C_DeleteFloorBlockHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_DeleteFloorBlock deleteFloorBlockPacket = packet as C_DeleteFloorBlock;

        GameRoom room = clientSession.MyPlayer.Room;
        if (room == null) return;
        if (room.Content.GameName != GameType.DoNotFall) return;
        DoNotFall game = room.Content as DoNotFall;
        room.Push(game.DeleteFloorBlock, deleteFloorBlockPacket.FloorIndex, deleteFloorBlockPacket.BlockIndex);
    }

    public static void C_PlayerDeadHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;

        GameRoom room = clientSession.MyPlayer.Room;
        if (room == null) return;
        room.Push(room.Content.PlayerDead, clientSession.MyPlayer.Id);
    }

    public static void C_CollideObstacleHandler(PacketSession session, IMessage packet)
    {
        Console.WriteLine("CollideObstacleHandler");
        ClientSession clientSession = session as ClientSession;

        GameRoom room = clientSession.MyPlayer.Room;
        if (room == null) return;
        SpeedRun game = room.Content as SpeedRun;
        if (game == null) return;
        room.Push(game.CollideObstacle, clientSession.MyPlayer);
    }

    public static void C_SetSpawnPointHandler(PacketSession session, IMessage packet)
    {
        Console.WriteLine("SetSpawnPointHandler");
        ClientSession clientSession = session as ClientSession;
        C_SetSpawnPoint setSpawnPointPacket = new C_SetSpawnPoint();
        clientSession.MyPlayer.SpawnPoint = setSpawnPointPacket.SpawnPoint;
    }
}
