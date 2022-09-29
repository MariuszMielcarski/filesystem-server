namespace FilesystemServer.Authorization;

using System.Net;
using System.Net.Http.Headers;
using System.Text;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentialString = Encoding.UTF8.GetString(credentialBytes);
            var credentials = credentialString.Split(':', 2);
            var username = credentials[0];
            var password = credentials[1];

            if (username != "admin" && password != "trudnehaslo")
            {
                throw new Exception("Wrong credentials.");
            }
        }
        catch
        {
            context.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"filesystemserver\"");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Access forbidded.");
            return;
        }

        await _next(context);
    }
}