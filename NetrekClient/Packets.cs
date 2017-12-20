using Godot;
using NetrekGodot.NetrekClient;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NetrekClient
{
    public static class Packets
    {
        public static IServerPacket ReadPacket(this BinaryReader reader)
        {
            byte t = reader.ReadByte();
            MemberInfo info = typeof(Packets).GetMembers().FirstOrDefault(m => m.GetCustomAttribute<PacketAttribute>() != null && ((Type)m).GetInterfaces().Contains(typeof(IServerPacket)) && m.GetCustomAttribute<PacketAttribute>().packetNum == t);
            if (info == null)
            {
                throw new InvalidDataException($"Invalid packet type {t} receieved");
            }
            Type type = info as Type;
            int size = Marshal.SizeOf(type);
            byte[] data = new byte[size + 1];
            data[0] = t;
            reader.ReadBytes(size).CopyTo(data, 1);
            // Allocate some space for the struct
            IntPtr pointer = Marshal.AllocHGlobal(size);
            // Copy data into struct
            Marshal.Copy(data, 0, pointer, size);
            IServerPacket packet = (IServerPacket)Marshal.PtrToStructure(pointer, type);
            // Free the pointer
            Marshal.FreeHGlobal(pointer);
            return packet;
        }
        internal static void WritePacket(this BinaryWriter writer, IClientPacket packet)
        {
            // Get size of packet
            int size = Marshal.SizeOf(packet);
            byte[] data = new byte[size];

            IntPtr pointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(packet, pointer, true);
            Marshal.Copy(pointer, data, 0, size);
            Marshal.FreeHGlobal(pointer);
            // Force type to be the actual packet type
            data[0] = packet.GetType().GetCustomAttribute<PacketAttribute>().packetNum;
            for (int i = 0; i < data.Length; i++)
            {
                Console.Write($"{data[i]} ");
            }
            Console.WriteLine();
            // Write data
            writer.Write(data);
        }

        public enum Client
        {
            NULL0,
            ///<summary>send a message</summary>
            CP_MESSAGE,
            ///<summary>set speed</summary>
            CP_SPEED,
            ///<summary>change direction</summary>
            CP_DIRECTION,
            ///<summary>phaser in a direction</summary>
            CP_PHASER,
            ///<summary>plasma (in a direction)</summary>
            CP_PLASMA,
            ///<summary>fire torp in a direction</summary>
            CP_TORP,
            ///<summary>self destruct</summary>
            CP_QUIT,
            ///<summary>log in (name, password)</summary>
            CP_LOGIN,
            ///<summary>outfit to new ship</summary>
            CP_OUTFIT,
            ///<summary>change war status</summary>
            CP_WAR,
            ///<summary>create practice robot, transwarp</summary>
            CP_PRACTR,
            ///<summary>raise/lower sheilds</summary>
            CP_SHIELD,
            ///<summary>enter repair mode</summary>
            CP_REPAIR,
            ///<summary>orbit planet/starbase</summary>
            CP_ORBIT,
            ///<summary>lock on planet</summary>
            CP_PLANLOCK,
            ///<summary>lock on player</summary>
            CP_PLAYLOCK,
            ///<summary>bomb a planet</summary>
            CP_BOMB,
            ///<summary>beam armies up/down</summary>
            CP_BEAM,
            ///<summary>cloak on/off</summary>
            CP_CLOAK,
            ///<summary>detonate enemy torps</summary>
            CP_DET_TORPS,
            ///<summary>detonate one of my torps</summary>
            CP_DET_MYTORP,
            ///<summary>toggle copilot mode</summary>
            CP_COPILOT,
            ///<summary>refit to different ship type</summary>
            CP_REFIT,
            ///<summary>tractor on/off</summary>
            CP_TRACTOR,
            ///<summary>pressor on/off</summary>
            CP_REPRESS,
            ///<summary>coup home planet</summary>
            CP_COUP,
            ///<summary>new socket for reconnection</summary>
            CP_SOCKET,
            ///<summary>send my options to be saved</summary>
            CP_OPTIONS,
            ///<summary>I'm done!</summary>
            CP_BYE,
            ///<summary>set docking permissions</summary>
            CP_DOCKPERM,
            ///<summary>set number of usecs per update</summary>
            CP_UPDATES,
            ///<summary>reset my stats packet</summary>
            CP_RESETSTATS,
            ///<summary>for future use</summary>
            CP_RESERVED,
            ///<summary>ATM: request for player scan</summary>
            CP_SCAN,
            ///<summary>request UDP on/off</summary>
            CP_UDP_REQ,
            ///<summary>sequence # packet</summary>
            CP_SEQUENCE,
            ///<summary>handles binary verification</summary>
            CP_RSA_KEY,
            ///<summary>cross-check planet info</summary>
            CP_PLANET,
            NULL39,
            NULL40,
            NULL41,
            /// <summary>client response</summary>
            CP_PING_RESPONSE,
            CP_S_REQ,
            CP_S_THRS,
            /// <summary>vari. Message Packet</summary>
            CP_S_MESSAGE,
            CP_S_RESERVED,
            CP_S_DUMMY,
            NULL48,
            NULL49,
            CP_OGGV,
            CP_MASTER_COMM,
            NULL52,
            NULL53,
            NULL54,
            NULL55,
            NULL56,
            NULL57,
            NULL58,
            NULL59,
            CP_FEATURE
        }
        public enum Server
        {
            NULL0,
            SP_MESSAGE,
            ///<summary>general player info not elsewhere</summary>
            SP_PLAYER_INFO,
            ///<summary># kills a player has</summary>
            SP_KILLS,
            ///<summary>x,y for player</summary>
            SP_PLAYER,
            ///<summary>torp status</summary>
            SP_TORP_INFO,
            ///<summary>torp location</summary>
            SP_TORP,
            ///<summary>phaser status and direction</summary>
            SP_PHASER,
            ///<summary>player login information</summary>
            SP_PLASMA_INFO,
            ///<summary>like SP_TORP</summary>
            SP_PLASMA,
            ///<summary>like SP_MESG</summary>
            SP_WARNING,
            ///<summary>line from .motd screen</summary>
            SP_MOTD,
            ///<summary>info on you?</summary>
            SP_YOU,
            ///<summary>estimated loc in queue?</summary>
            SP_QUEUE,
            ///<summary>galaxy status numbers</summary>
            SP_STATUS,
            ///<summary>planet armies & facilities</summary>
            SP_PLANET,
            ///<summary>your team & ship was accepted</summary>
            SP_PICKOK,
            ///<summary>login response</summary>
            SP_LOGIN,
            ///<summary>give flags for a player</summary>
            SP_FLAGS,
            ///<summary>tournament mode mask</summary>
            SP_MASK,
            ///<summary>give status for a player</summary>
            SP_PSTATUS,
            ///<summary>invalid version number</summary>
            SP_BADVERSION,
            ///<summary>hostility settings for a player</summary>
            SP_HOSTILE,
            ///<summary>a player's statistics</summary>
            SP_STATS,
            ///<summary>new player logs in</summary>
            SP_PL_LOGIN,
            ///<summary>for future use</summary>
            SP_RESERVED,
            ///<summary>planet name, x, y</summary>
            SP_PLANET_LOC,
            NULL27,
            ///<summary>notify client of UDP status</summary>
            SP_UDP_REPLY,
            ///<summary>sequence # packet</summary>
            SP_SEQUENCE,
            ///<summary>this trans is semi-critical info</summary>
            SP_SC_SEQUENCE,
            ///<summary>handles binary verification</summary>
            SP_RSA_KEY,
            ///<summary>32 byte generic, see struct</summary>
            SP_GENERIC_32,
            ///<summary>abbreviated flags for all players</summary>
            SP_FLAGS_ALL,
            NULL34,
            NULL35,
            NULL36,
            NULL37,
            NULL38,
            ///<summary>Handles server ship mods</summary>
            SP_SHIP_CAP,
            ///<summary>reply to send-short request</summary>
            SP_S_REPLY,
            ///<summary>var. Message Packet</summary>
            SP_S_MESSAGE,
            ///<summary>Warnings with 4  Bytes</summary>
            SP_S_WARNING,
            ///<summary>hostile,armies,whydead,etc ..</summary>
            SP_S_YOU,
            ///<summary>your ship status</summary>
            SP_S_YOU_SS,
            ///<summary>variable length player packet</summary>
            SP_S_PLAYER,
            ///<summary>ping packet</summary>
            SP_PING,
            ///<summary>variable length torp packet</summary>
            SP_S_TORP,
            ///<summary>SP_S_TORP with TorpInfo</summary>
            SP_S_TORP_INFO,
            ///<summary>optimized SP_S_TORP</summary>
            SP_S_8_TORP,
            ///<summary>see SP_PLANET</summary>
            SP_S_PLANET,
            NULL51,
            NULL52,
            NULL53,
            NULL54,
            NULL55,
            ///<summary>SP_SEQUENCE for compressed packets</summary>
            SP_S_SEQUENCE,
            ///<summary>see struct</summary>
            SP_S_PHASER,
            ///<summary># of kills player have</summary>
            SP_S_KILLS,
            ///<summary>see SP_STATS</summary>
            SP_S_STATS,
            SP_FEATURE,
            SP_RANK,
            ///<summary>LTD stats for byteacter</summary>
            SP_LTD,
        }

#pragma warning disable IDE1006 // Naming Styles
        [Packet(Server.SP_MESSAGE)]
        public struct mesg_spacket : IServerPacket
        {
            public byte type;      /* SP_MESSAGE */
            public byte m_flags;
            public byte m_recpt;
            public byte m_from;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=80)]
            public byte[] mesg;
        };
        [Packet(Server.SP_PLAYER_INFO)]
        public struct plyr_info_spacket : IServerPacket
        {
            public byte type;      /* SP_PLAYER_INFO */
            public byte pnum;
            public byte shiptype;
            public byte team;
        };
        [Packet(Server.SP_PL_LOGIN)]
        public struct plyr_login_spacket : IServerPacket
        {
            public byte type;      /* SP_PL_LOGIN */
            public byte pnum;
            public byte rank;
            public byte pad1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            public byte[] name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            public byte[] monitor;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            public byte[] login;
        };
        [Packet(Server.SP_HOSTILE)]
        public struct hostile_spacket : IServerPacket
        {
            public byte type;      /* SP_HOSTILE */
            public byte pnum;
            public byte war;
            public byte hostile;
        };
        [Packet(Server.SP_STATS)]
        public struct stats_spacket : IServerPacket
        {
            public byte type;      /* SP_STATS */
            public byte pnum;
            public byte pad1;
            public byte pad2;
            public int tkills; /* Tournament kills */
            public int tlosses;    /* Tournament losses */
            public int kills;      /* overall */
            public int losses; /* overall */
            public int tticks; /* ticks of tournament play time */
            public int tplanets;   /* Tournament planets */
            public int tarmies;    /* Tournament armies */
            public int sbkills;    /* Starbase kills */
            public int sblosses;   /* Starbase losses */
            public int armies; /* non-tourn armies */
            public int planets;    /* non-tourn planets */
            public int maxkills;   /* max kills as player * 100 */
            public int sbmaxkills; /* max kills as sb * 100 */
        };
        [Packet(Server.SP_FLAGS)]
        public struct flags_spacket : IServerPacket
        {
            public byte type;      /* SP_FLAGS */
            public byte pnum;      /* whose flags are they? */
            public byte pad1;
            public byte pad2;
            public int flags;
        };
        [Packet(Server.SP_KILLS)]
        public struct kills_spacket : IServerPacket
        {
            public byte type;      /* SP_KILLS */
            public byte pnum;
            public byte pad1;
            public byte pad2;
            public int kills; /* where 1234=12.34 kills and 0=0.00 kills */
        };
        [Packet(Server.SP_PLAYER)]
        public struct player_spacket : IServerPacket
        {
            public byte type;      /* SP_PLAYER */
            public byte pnum;
            public byte dir;
            public byte speed;
            public int x, y;
        };
        [Packet(Server.SP_TORP_INFO)]
        public struct torp_info_spacket : IServerPacket
        {
            public byte type;      /* SP_TORP_INFO */
            public byte war;
            public byte status;    /* TFREE, TDET, etc... */
            public byte pad1;      /* pad needed for cross cpu compatibility */
            public short tnum;
            public short pad2;
        };
        [Packet(Server.SP_TORP)]
        public struct torp_spacket : IServerPacket
        {
            public byte type;      /* SP_TORP */
            public byte dir;
            public short tnum;
            public int x, y;
        };
        [Packet(Server.SP_PHASER)]
        public struct phaser_spacket : IServerPacket
        {
            public byte type;      /* SP_PHASER */
            public byte pnum;
            public byte status;    /* PH_HIT, etc... */
            public byte dir;
            public int x, y;
            public int target;
        };
        [Packet(Server.SP_YOU)]
        public struct you_spacket : IServerPacket
        {
            public byte type;      /* SP_YOU */
            public byte pnum;      /* Guy needs to know this... */
            public byte hostile;
            public byte swar;
            public byte armies;
            public byte pad1;
            public byte pad2;
            public byte pad3;
            public int flags;
            public int damage;
            public int shield;
            public int fuel;
            public short etemp;
            public short wtemp;
            public short whydead;
            public short whodead;
        };
        [Packet(Server.SP_STATUS)]
        public struct status_spacket : IServerPacket
        {
            public byte type;      /* SP_STATUS */
            public byte tourn;
            public byte pad1;
            public byte pad2;
            public int armsbomb;
            public int planets;
            public int kills;
            public int losses;
            public int time;
            public int timeprod;
        };
        [Packet(Server.SP_WARNING)]
        public struct warning_spacket : IServerPacket
        {
            public byte type;      /* SP_WARNING */
            public byte pad1;
            public byte pad2;
            public byte pad3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=80)]
            public byte[] mesg;
        };
        [Packet(Server.SP_PLANET)]
        public struct planet_spacket : IServerPacket
        {
            public byte type;      /* SP_PLANET */
            public byte pnum;
            public byte owner;
            public byte info;
            public short flags;
            public short pad2;
            public int armies;
        };
        [Packet(Client.CP_TORP)]
        public struct torp_cpacket : IClientPacket
        {
            public byte type;      /* CP_TORP */
            public byte dir;      /* direction to fire torp */
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_PHASER)]
        public struct phaser_cpacket : IClientPacket
        {
            public byte type;      /* CP_PHASER */
            public byte dir;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_SPEED)]
        public struct speed_cpacket : IClientPacket
        {
            public byte type;      /* CP_SPEED */
            public byte speed;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_DIRECTION)]
        public struct dir_cpacket : IClientPacket
        {
            public byte type;      /* CP_DIRECTION */
            public byte dir;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_SHIELD)]
        public struct shield_cpacket : IClientPacket
        {
            public byte type;      /* CP_SHIELD */
            public byte state;     /* up/down */
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_REPAIR)]
        public struct repair_cpacket : IClientPacket
        {
            public byte type;      /* CP_REPAIR */
            public byte state;     /* on/off */
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_ORBIT)]
        public struct orbit_cpacket : IClientPacket
        {
            public byte type;      /* CP_ORBIT */
            public byte state;     /* on/off */
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_PRACTR)]
        public struct practr_cpacket : IClientPacket
        {
            public byte type;      /* CP_PRACTR */
            public byte pad1;
            public byte pad2;
            public byte pad3;
        };
        [Packet(Client.CP_BOMB)]
        public struct bomb_cpacket : IClientPacket
        {
            public byte type;      /* CP_BOMB */
            public byte state;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_BEAM)]
        public struct beam_cpacket : IClientPacket
        {
            public byte type;      /* CP_BEAM */
            public byte state;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_CLOAK)]
        public struct cloak_cpacket : IClientPacket
        {
            public byte type;      /* CP_CLOAK */
            public byte state;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_DET_TORPS)]
        public struct det_torps_cpacket : IClientPacket
        {
            public byte type;      /* CP_DET_TORPS */
            public byte pad1;
            public byte pad2;
            public byte pad3;
        };
        [Packet(Client.CP_COPILOT)]
        public struct copilot_cpacket : IClientPacket
        {
            public byte type;      /* CP_COPLIOT */
            public byte state;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Server.SP_QUEUE)]
        public struct queue_spacket : IServerPacket
        {
            public byte type;      /* SP_QUEUE */
            public byte pad1;
            public short pos;
        };
        [Packet(Client.CP_OUTFIT)]
        public struct outfit_cpacket : IClientPacket
        {
            public byte type;      /* CP_OUTFIT */
            public byte team;
            public byte ship;
            public byte pad1;
        };
        [Packet(Server.SP_PICKOK)]
        public struct pickok_spacket : IServerPacket
        {
            public byte type;      /* SP_PICKOK */
            public byte state;
            public byte pad2;
            public byte pad3;
        };
        [Packet(Client.CP_LOGIN)]
        public struct login_cpacket : IClientPacket
        {
            public byte type;      /* CP_LOGIN */
            public byte query;
            public byte pad2;
            public byte pad3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            public byte[] name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            public byte[] password;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            public byte[] login;
        };
        [Packet(Server.SP_LOGIN)]
        public struct login_spacket : IServerPacket
        {
            public byte type;      /* SP_LOGIN */
            public byte accept;    /* 1/0 */
            public byte pad2;
            public byte pad3;
            public int flags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=96)]
            public byte[] keymap;
        };
        [Packet(Client.CP_TRACTOR)]
        public struct tractor_cpacket : IClientPacket
        {
            public byte type;      /* CP_TRACTOR */
            public byte state;
            public byte pnum;
            public byte pad2;
        };
        [Packet(Client.CP_REPRESS)]
        public struct repress_cpacket : IClientPacket
        {
            public byte type;      /* CP_REPRESS */
            public byte state;
            public byte pnum;
            public byte pad2;
        };
        [Packet(Client.CP_DET_MYTORP)]
        public struct det_mytorp_cpacket : IClientPacket
        {
            public byte type;      /* CP_DET_MYTORP */
            public byte pad1;
            public short tnum;
        };
        [Packet(Client.CP_WAR)]
        public struct war_cpacket : IClientPacket
        {
            public byte type;      /* CP_WAR */
            public byte newmask;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_REFIT)]
        public struct refit_cpacket : IClientPacket
        {
            public byte type;      /* CP_REFIT */
            public byte ship;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_PLASMA)]
        public struct plasma_cpacket : IClientPacket
        {
            public byte type;      /* CP_PLASMA */
            public byte dir;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Server.SP_PLASMA_INFO)]
        public struct plasma_info_spacket : IServerPacket
        {
            public byte type;      /* SP_PLASMA_INFO */
            public byte war;
            public byte status;    /* TFREE, TDET, etc... */
            public byte pad1;      /* pad needed for cross cpu compatibility */
            public short pnum;
            public short pad2;
        };
        [Packet(Server.SP_PLASMA)]
        public struct plasma_spacket : IServerPacket
        {
            public byte type;      /* SP_PLASMA */
            public byte pad1;
            public short pnum;
            public int x, y;
        };
        [Packet(Client.CP_PLAYLOCK)]
        public struct playlock_cpacket : IClientPacket
        {
            public byte type;      /* CP_PLAYLOCK */
            public byte pnum;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_PLANLOCK)]
        public struct planlock_cpacket : IClientPacket
        {
            public byte type;      /* CP_PLANLOCK */
            public byte pnum;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_COUP)]
        public struct coup_cpacket : IClientPacket
        {
            public byte type;      /* CP_COUP */
            public byte pad1;
            public byte pad2;
            public byte pad3;
        };
        [Packet(Server.SP_PSTATUS)]
        public struct pstatus_spacket : IServerPacket
        {
            public byte type;      /* SP_PSTATUS */
            public byte pnum;
            public byte status;
            public byte pad1;
        };
        [Packet(Server.SP_MOTD)]
        public struct motd_spacket : IServerPacket
        {
            public byte type;      /* SP_MOTD */
            public byte pad1;
            public byte pad2;
            public byte pad3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=80)]
            public byte[] line;
        };
        [Packet(Client.CP_QUIT)]
        public struct quit_cpacket : IClientPacket
        {
            public byte type;      /* CP_QUIT */
            public byte pad1;
            public byte pad2;
            public byte pad3;
        };
        [Packet(Client.CP_MESSAGE)]
        public struct mesg_cpacket : IClientPacket
        {
            public byte type;      /* CP_MESSAGE */
            public byte group;
            public byte indiv;
            public byte pad1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=80)]
            public byte[] mesg;
        };
        [Packet(Server.SP_MASK)]
        public struct mask_spacket : IServerPacket
        {
            public byte type;      /* SP_MASK */
            public byte mask;
            public byte pad1;
            public byte pad2;
        };
        [Packet(Client.CP_SOCKET)]
        public struct socket_cpacket : IClientPacket
        {
            public byte type;      /* CP_SOCKET */
            public byte version;
            public byte udp_version;   /* was pad2 */
            public byte pad3;
            public int socket;
        };
        [Packet(Client.CP_OPTIONS)]
        public struct options_cpacket : IClientPacket
        {
            public byte type;      /* CP_OPTIONS */
            public byte pad1;
            public byte pad2;
            public byte pad3;
            public int flags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=96)]
            public byte[] keymap;
        };
        [Packet(Client.CP_BYE)]
        public struct bye_cpacket : IClientPacket
        {
            public byte type;      /* CP_BYE */
            public byte pad1;
            public byte pad2;
            public byte pad3;
        };
        [Packet(Server.SP_BADVERSION)]
        public struct badversion_spacket : IServerPacket
        {
            public byte type;      /* SP_BADVERSION */
            public byte why;
            public byte pad2;
            public byte pad3;
        };
        [Packet(Client.CP_DOCKPERM)]
        public struct dockperm_cpacket : IClientPacket
        {
            public byte type;      /* CP_DOCKPERM */
            public byte state;
            public byte pad2;
            public byte pad3;
        };
        [Packet(Client.CP_UPDATES)]
        public struct updates_cpacket : IClientPacket
        {
            public byte type;      /* CP_UPDATES */
            public byte pad1;
            public byte pad2;
            public byte pad3;
            public int usecs;
        };
        [Packet(Client.CP_RESETSTATS)]
        public struct resetstats_cpacket : IClientPacket
        {
            public byte type;      /* CP_RESETSTATS */
            public byte verify;    /* 'Y' - just to make sure he meant it */
            public byte pad2;
            public byte pad3;
        };
        [Packet(Server.SP_RESERVED)]
        public struct reserved_spacket : IServerPacket
        {
            public byte type;      /* SP_RESERVED */
            public byte pad1;
            public byte pad2;
            public byte pad3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            public byte[] data;
        };
        [Packet(Client.CP_RESERVED)]
        public struct reserved_cpacket : IClientPacket
        {
            public byte type;      /* CP_RESERVED */
            public byte pad1;
            public byte pad2;
            public byte pad3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            public byte[] data;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            public byte[] resp;
        };
        [Packet(Client.CP_UDP_REQ)]
        public struct udp_req_cpacket : IClientPacket
        {        /* UDP */
            public byte type;          /* CP_UDP_REQ */
            public byte request;
            public byte connmode;      /* respond with port # or just send UDP packet? */
            public byte pad2;
            public int port;          /* compensate for hosed recvfrom() */
        };
        [Packet(Client.CP_SEQUENCE)]
        public struct sequence_cpacket : IClientPacket
        {       /* UDP */
            public byte type;          /* CP_SEQUENCE */
            public byte pad1;
            public ushort sequence;
        };
        [Packet(Client.CP_PING_RESPONSE)]
        public struct ping_cpacket : IClientPacket
        {
            public byte type;           /* CP_PING_RESPONSE */
            public byte number;         /* id */
            public byte pingme;         /* if client wants server to ping */
            public byte pad1;

            public int cp_sent;        /* # packets sent to server */
            public int cp_recv;        /* # packets recv from server */
        };
        [Packet(Server.SP_PING)]
        public struct ping_spacket : IServerPacket
        {
            public byte type;           /* SP_PING */
            public byte number;         /* id */
            public ushort lag;            /* delay in ms */

            public byte tloss_sc;       /* total loss server-client 0-100% */
            public byte tloss_cs;       /* total loss client-server 0-100% */

            public byte iloss_sc;       /* inc. loss server-client 0-100% */
            public byte iloss_cs;       /* inc. loss client-server 0-100% */
        };
        [Packet(Client.CP_S_REQ)]
        public struct shortreq_cpacket : IClientPacket
        {   /* CP_S_REQ */
            public byte type;
            public byte req;
            public byte pad1, pad2;
        };
        [Packet(Server.SP_S_REPLY)]
        public struct shortreply_spacket : IServerPacket
        {   /* SP_S_REPLY */
            public byte type;
            public byte repl;
            public byte pad1, pad2;
        };
        [Packet(Server.SP_S_YOU)]
        public struct youshort_spacket : IServerPacket
        {   /* SP_S_YOU */
            public byte type;

            public byte pnum;
            public byte hostile;
            public byte swar;

            public byte armies;
            public byte whydead;
            public byte whodead;

            public byte pad1;

            public int flags;
        };
        [Packet(Server.SP_S_YOU_SS)]
        public struct youss_spacket : IServerPacket
        {       /* SP_S_YOU_SS */
            public byte type;
            public byte pad1;

            public ushort damage;
            public ushort shield;
            public ushort fuel;
            public ushort etemp;
            public ushort wtemp;
        };
        [Packet(Server.SP_UDP_REPLY)]
        public struct udp_reply_spacket : IServerPacket
        {      /* UDP */
            public byte type;          /* SP_UDP_REPLY */
            public byte reply;
            public byte pad1;
            public byte pad2;
            public int port;
        };
        [Packet(Server.SP_SEQUENCE)]
        public struct sequence_spacket : IServerPacket
        {       /* UDP */
            public byte type;          /* SP_SEQUENCE */
            public byte pad1;
            public ushort sequence;
        };

        [Packet(Server.SP_PLANET_LOC)]
        public struct planet_loc_spacket : IServerPacket
        {
            public byte type;      /* SP_PLANET_LOC */
            public byte pnum;
            public byte pad2;
            public byte pad3;
            public int x;
            public int y;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            public byte[] name;

        };
        [Packet(Client.CP_OGGV)]
        public struct oggv_cpacket : IClientPacket
        {
            public byte type;   /* CP_OGGV */
            public byte def;  /* defense 1-100 */
            public byte targ;   /* target */
        };
        [Packet(Server.SP_FEATURE)]
        public struct feature_spacket : IServerPacket
        {
            public byte type;
            public byte feature_type;
            public byte arg1;
            public byte arg2;
            public int value;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=80)]
            public byte[] name;
        };
        [Packet(Client.CP_FEATURE)]
        public struct feature_cpacket : IClientPacket
        {
            public byte type;
            public byte feature_type;
            public byte arg1;
            public byte arg2;
            public int value;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=80)]
            public byte[] name;
        };




#pragma warning restore IDE1006 // Naming Styles
        // This allows us to auto-fill the type in the writer
        public class PacketAttribute : Attribute
        {
            public byte packetNum;
            public PacketAttribute(byte packetNum)
            {
                this.packetNum = packetNum;
            }
            public PacketAttribute(Server packetNum) : this((byte)packetNum)
            {

            }
            public PacketAttribute(Client packetNum) : this((byte)packetNum)
            {

            }
        }

    }
}
