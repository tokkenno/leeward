using System;
using System.ComponentModel;
using System.Text;

namespace Leeward.Protocol
{
    internal enum PacketType : uint
    {
        Empty = 0,
        Error = 1,
        Disconnect = 2,
        RequestID = 3,
        RequestPing = 4,
        RequestSetUDP = 5,
        RequestJoinChannel = 6,
        RequestLeaveChannel = 7,
        RequestCloseChannel = 8,
        RequestSetPlayerLimit = 9,
        RequestLoadLevel = 10,           // 0x0000000A
        RequestSetName = 11,             // 0x0000000B
        RequestSetHost = 12,             // 0x0000000C
        RequestRemoveRFC = 13,           // 0x0000000D
        RequestCreate = 14,              // 0x0000000E
        RequestDestroy = 15,             // 0x0000000F
        RequestSaveFile = 16,            // 0x00000010
        RequestLoadFile = 17,            // 0x00000011
        RequestDeleteFile = 18,          // 0x00000012
        RequestNoDelay = 19,             // 0x00000013
        RequestSetChannelData = 20,      // 0x00000014
        RequestChannelList = 21,         // 0x00000015
        ResponseID = 22,                 // 0x00000016
        ResponsePing = 23,               // 0x00000017
        ResponseSetUDP = 24,             // 0x00000018
        ResponsePlayerLeft = 25,         // 0x00000019
        ResponsePlayerJoined = 26,       // 0x0000001A
        ResponseJoiningChannel = 27,     // 0x0000001B
        ResponseJoinChannel = 28,        // 0x0000001C
        ResponseLeaveChannel = 29,       // 0x0000001D
        ResponseRenamePlayer = 30,       // 0x0000001E
        ResponseSetHost = 31,            // 0x0000001F
        ResponseLoadLevel = 32,          // 0x00000020
        ResponseCreate = 33,             // 0x00000021
        ResponseDestroy = 34,            // 0x00000022
        ResponseLoadFile = 35,           // 0x00000023
        ResponseSetChannelData = 36,     // 0x00000024
        ResponseChannelList = 37,        // 0x00000025
        ForwardToAll = 38,               // 0x00000026
        ForwardToAllSaved = 39,          // 0x00000027
        ForwardToOthers = 40,            // 0x00000028
        ForwardToOthersSaved = 41,       // 0x00000029
        ForwardToHost = 42,              // 0x0000002A
        ForwardToPlayer = 43,            // 0x0000002B
        ForwardToPlayerSaved = 44,       // 0x0000002C
        RequestAddServer = 45,           // 0x0000002D
        RequestRemoveServer = 46,        // 0x0000002E
        RequestServerList = 47,          // 0x0000002F
        ResponseServerList = 48,         // 0x00000030
        RequestSetTimeout = 49,          // 0x00000031
        Broadcast = 50,                  // 0x00000032
        RequestActivateUDP = 51,         // 0x00000033
        SyncPlayerData = 52,             // 0x00000034
        ForwardByName = 53,              // 0x00000035
        ForwardTargetNotFound = 54,      // 0x00000036
        ServerLog = 55,                  // 0x00000037
        RequestDeleteChannel = 56,       // 0x00000038
        RequestVerifyAdmin = 57,         // 0x00000039
        RequestCreateAdmin = 58,         // 0x0000003A
        RequestRemoveAdmin = 59,         // 0x0000003B
        RequestKick = 60,                // 0x0000003C
        RequestBan = 61,                 // 0x0000003D
        RequestSetAlias = 62,            // 0x0000003E
        RequestUnban = 63,               // 0x0000003F
        RequestLogPlayers = 64,          // 0x00000040
        RequestSetBanList = 65,          // 0x00000041
        ResponseReloadServerConfig = 66, // 0x00000042
        RequestSetServerOption = 67,     // 0x00000043
        ResponseSetServerOption = 68,    // 0x00000044
        RequestReloadServerConfig = 69,  // 0x00000045
        BroadcastAdmin = 70,             // 0x00000046
        ResponseVerifyAdmin = 71,        // 0x00000047
        RequestGetFileList = 72,         // 0x00000048
        ResponseGetFileList = 73,        // 0x00000049
        RequestLockChannel = 74,         // 0x0000004A
        ResponseLockChannel = 75,        // 0x0000004B
        PlayerConnected = 76,            // 0x0000004C
        PlayerDisconnected = 77,         // 0x0000004D
        RequestHttpGet = 78,             // 0x0000004E
        UserPacket = 128,                // 0x00000080
        HttpGet = 0x20544547,            // HTTP Request: GET
        HttpPost = 0x54534f50            // HTTP Request: POST
    }
}