// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using Kaenx.Konnect.Addresses;

Console.WriteLine("KNX Penetrations Test");

Console.WriteLine("Schnittstelle für Tunneling angeben");
Console.Write("IP-Adresse: ");
string ip = Console.ReadLine() ?? "";
Console.Write("Port (3671): ");
string portx = Console.ReadLine() ?? "";
if(string.IsNullOrEmpty(portx)) portx = "3671";
int port = int.Parse(portx);
Console.WriteLine("Test Parameter angeben");
Console.Write("Anzahl Tunnel (10): ");
string countx = Console.ReadLine() ?? "";
if(string.IsNullOrEmpty(countx)) countx = "10";
int count = int.Parse(countx);
Console.Write("Größe der Payload (1): ");
string sizex = Console.ReadLine() ?? "";
if(string.IsNullOrEmpty(sizex)) sizex = "1";
int size = int.Parse(sizex);
Console.Write("Verwendete Gruppenadresse (15/7/255): ");
string groupx = Console.ReadLine() ?? "";
if(string.IsNullOrEmpty(groupx)) groupx = "15/7/255";
MulticastAddress group = MulticastAddress.FromString(groupx);
Console.Write("Daten senden? (n) [j/n] ");
bool send = (Console.ReadLine() ?? "") == "j";


byte[] data = new byte[size];
Random.Shared.NextBytes(data);

List<Kaenx.Konnect.Connections.KnxIpTunneling> conns = new();
List<Kaenx.Konnect.Classes.BusCommon> tunnels = new();

Console.WriteLine($"");
Console.WriteLine($"Verbindungen werden aufgebaut");
for(int i = 1; i <= count; i++)
{
    try{
        Kaenx.Konnect.Connections.KnxIpTunneling conn = new (ip, port);
        await conn.Connect();
        conns.Add(conn);
        tunnels.Add(new(conn));
        Console.WriteLine($"Tunnel {i} ist verbunden.");
    } catch(Exception ex)
    {
        Console.WriteLine($"Tunnel {i} konnte nicht aufgebaut werden.");
        Console.WriteLine(ex.Message);
    }
}

Console.CancelKeyPress += delegate {
    foreach(Kaenx.Konnect.Connections.KnxIpTunneling conn in conns)
        conn.Disconnect();
};

if(tunnels.Count == 0)
{
    Console.WriteLine("Kein Tunnel zum Testen vorhanden.");
    return;
}

while(true)
{
    if(send)
    {
        foreach(Kaenx.Konnect.Classes.BusCommon tun in tunnels)
        {
            _ = tun.GroupValueWrite(group, data);
        }
    }
}