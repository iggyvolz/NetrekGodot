using Godot;
using NetrekGodot.NetrekClient;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace NetrekClient
{
    public class Client
    {
        private StreamPeerTCPStream tcp;
        private BinaryReader reader;
        private BinaryWriter writer;
        public bool HasMessage
        {
            get
            {
                GD.Print(tcp.AvailableBytes);
                return tcp.AvailableBytes > 0;
            }
        }
        public Client(string ip, int port)
        {
            StreamPeerTCP t = new StreamPeerTCP();
            GodotError.Assert(t.ConnectToHost(ip, port)); // Always gives zero
            tcp = new StreamPeerTCPStream(t);
            reader = new BinaryReader(tcp);
            writer = new BinaryWriter(tcp);
        }
        public void SendMessage(IClientPacket packet)
        {
            try
            {
                writer.WritePacket(packet);
                writer.Flush();
                //GD.Print("OK");
            }
            catch (GodotError e)
            {
                //GD.Print($"Caught {e.Name}");
            }
        }
        public IServerPacket ReceiveMessage()
        {
            //try
            //{
                IServerPacket p = reader.ReadPacket();
                //Console.WriteLine("P CHECK UPCOMING");
                //Console.WriteLine(p == null);
                //Console.WriteLine("P UPCOMING");
                //Console.WriteLine(p);
                return p;
            //}
            //catch(NullReferenceException e)
            //{
            //    Console.WriteLine(e);
            //    throw new Exception();
            //}
        }
    }
}
