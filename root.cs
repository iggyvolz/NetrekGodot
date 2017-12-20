using Godot;
using NetrekClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using static NetrekClient.Packets;

public class root : Node2D
{
    NetrekClient.Client client;
    public override void _Ready()
    {
        client = new NetrekClient.Client("127.0.0.1", 2592);
        socket_cpacket packeta = new socket_cpacket()
        {
            version = 4,
            udp_version = 10,
            pad3 = 0,
            socket = 15320
        };
        //GD.Print(packeta.ToString());
        client.SendMessage(packeta);
        feature_cpacket packet = new feature_cpacket()
        {
            feature_type = 83,
            arg1 = 0,
            arg2 = 0,
            value = 1,
            name = Encoding.ASCII.GetBytes("FEATURE_PACKETS").TakeAll(80).ToArray()
        };
        //GD.Print(packet);
        client.SendMessage(packet);
        //GD.Print("Sent");
    }
    public bool erred = false;
    public override void _Process(float delta)
    {
        try
        {
            if (client.HasMessage && !erred)
            {
                IServerPacket packet = client.ReceiveMessage();
                return;
                if (packet is motd_spacket motd)
                {
                    GD.Print("MOTD: " + new string(motd.line.Select(b => (char)b).ToArray()));
                }
                else
                {
                    GD.Print($"Received {packet.GetType().ToString()}");
                }
            }
        }
        catch (Exception e)
        {
            erred = true;
            throw e;
        }
    }
}
public static class Extension
{
    public static IEnumerable<T> TakeAll<T>(this IEnumerable<T> arr, int count)
    {
        IEnumerable<T> taken = arr.Take(count);
        if (taken.Count() == count)
        {
            return taken;
        }
        T[] takenall = new T[count];
        taken.ToArray().CopyTo(takenall, 0);
        return takenall;
    }

    public static string ToString(this IPacket packet, bool _)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(packet.GetType().ToString());
        foreach (FieldInfo f in packet.GetType().GetFields())
        {
            builder.AppendLine($"{f.Name}: {f.GetValue(packet)}");
        }
        return builder.ToString();
    }
}
