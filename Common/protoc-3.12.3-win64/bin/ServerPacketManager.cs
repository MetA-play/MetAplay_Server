using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
		
	public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }

	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.CCreateRoomReq, MakePacket<C_CreateRoomReq>);
		_handler.Add((ushort)MsgId.CCreateRoomReq, PacketHandler.C_CreateRoomReqHandler);		
		_onRecv.Add((ushort)MsgId.CJoinRoomReq, MakePacket<C_JoinRoomReq>);
		_handler.Add((ushort)MsgId.CJoinRoomReq, PacketHandler.C_JoinRoomReqHandler);		
		_onRecv.Add((ushort)MsgId.CMove, MakePacket<C_Move>);
		_handler.Add((ushort)MsgId.CMove, PacketHandler.C_MoveHandler);		
		_onRecv.Add((ushort)MsgId.CChat, MakePacket<C_Chat>);
		_handler.Add((ushort)MsgId.CChat, PacketHandler.C_ChatHandler);		
		_onRecv.Add((ushort)MsgId.CUpdateGameStateReq, MakePacket<C_UpdateGameStateReq>);
		_handler.Add((ushort)MsgId.CUpdateGameStateReq, PacketHandler.C_UpdateGameStateReqHandler);		
		_onRecv.Add((ushort)MsgId.CSetUserinfo, MakePacket<C_SetUserinfo>);
		_handler.Add((ushort)MsgId.CSetUserinfo, PacketHandler.C_SetUserinfoHandler);		
		_onRecv.Add((ushort)MsgId.CHitSoccerball, MakePacket<C_HitSoccerball>);
		_handler.Add((ushort)MsgId.CHitSoccerball, PacketHandler.C_HitSoccerballHandler);		
		_onRecv.Add((ushort)MsgId.CSyncPos, MakePacket<C_SyncPos>);
		_handler.Add((ushort)MsgId.CSyncPos, PacketHandler.C_SyncPosHandler);		
		_onRecv.Add((ushort)MsgId.CDeleteFloorBlock, MakePacket<C_DeleteFloorBlock>);
		_handler.Add((ushort)MsgId.CDeleteFloorBlock, PacketHandler.C_DeleteFloorBlockHandler);		
		_onRecv.Add((ushort)MsgId.CPlayerDead, MakePacket<C_PlayerDead>);
		_handler.Add((ushort)MsgId.CPlayerDead, PacketHandler.C_PlayerDeadHandler);		
		_onRecv.Add((ushort)MsgId.CCollideObstacle, MakePacket<C_CollideObstacle>);
		_handler.Add((ushort)MsgId.CCollideObstacle, PacketHandler.C_CollideObstacleHandler);		
		_onRecv.Add((ushort)MsgId.CSetSpawnPoint, MakePacket<C_SetSpawnPoint>);
		_handler.Add((ushort)MsgId.CSetSpawnPoint, PacketHandler.C_SetSpawnPointHandler);		
		_onRecv.Add((ushort)MsgId.CCollideEndLine, MakePacket<C_CollideEndLine>);
		_handler.Add((ushort)MsgId.CCollideEndLine, PacketHandler.C_CollideEndLineHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

		if (CustomHandler != null)
		{
			CustomHandler.Invoke(session, pkt, id);
		}
		else
		{
			Action<PacketSession, IMessage> action = null;
			if (_handler.TryGetValue(id, out action))
				action.Invoke(session, pkt);
		}
	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}