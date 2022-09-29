namespace FilesystemServer.Authorization;

using System.Net;

public class IpFilterMiddleware
{
    private readonly RequestDelegate _next;

    private List<IPAddress> AllowedIps = new List<IPAddress>{
         IPAddress.Parse("::1")
    };

    public IpFilterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress;

        if (!AllowedIps.Contains(ip))
        {
            return;
        }

        await _next(context);
    }
}