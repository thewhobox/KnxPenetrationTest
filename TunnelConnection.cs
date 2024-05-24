

using Kaenx.Konnect.Messages.Request;
using Kaenx.Konnect.Messages.Response;

public class TunnelConnection
{
    public Kaenx.Konnect.Connections.KnxIpTunneling Connection { get; set; }
    public Kaenx.Konnect.Classes.BusCommon Tunnel { get; set; }
    public int Id { get; set; }

    public TunnelConnection(int id, Kaenx.Konnect.Connections.KnxIpTunneling conn, bool show = true)
    {
        Id = id;
        Connection = conn;
        Tunnel = new(conn);
        if(!show) return;
        Connection.OnTunnelRequest += TunnelRequestHandler;
        Connection.OnTunnelResponse += TunnelResponseHandler;
    }

    private void TunnelRequestHandler(IMessageRequest message)
    {
        Console.WriteLine($"[{Id}] Request: {message}");
    }

    private void TunnelResponseHandler(IMessageResponse message)
    {
        Console.WriteLine($"[{Id}] Response: {message}");
    }
}