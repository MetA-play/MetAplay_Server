protoc.exe -I=./ --csharp_out=./ ./Protocol.proto 
IF ERRORLEVEL 1 PAUSE

START ../../../MetAplay/ProtocolGenerator/bin/Debug/net6.0/ProtocolGenerator.exe ./Protocol.proto
XCOPY /Y Protocol.cs "../../../MetAplay/MetAplay/Packet"
XCOPY /Y ServerPacketManager.cs "../../../MetAplay/MetAplay/Packet"
