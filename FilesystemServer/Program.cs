using FilesystemServer.Authorization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (hostContext, services, configuration) => configuration
    .Enrich.WithThreadId()
    .Enrich.WithClientIp()
    .WriteTo.Console()
    .WriteTo.File("log.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} Thread: {ThreadId} Client: {ClientIp} [{Level:u3}] {Message:lj} Properties:{Properties}{NewLine}{Exception}"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<BasicAuthMiddleware>();
app.UseMiddleware<IpFilterMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Performance}/{action=GetStats}/{id?}");


app.Run();
