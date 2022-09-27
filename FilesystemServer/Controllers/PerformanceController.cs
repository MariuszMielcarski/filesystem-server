using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FilesystemServer.Controllers;

public class PerformanceController : ControllerBase
{
    private readonly ILogger<PerformanceController> _logger;

    public PerformanceController(ILogger<PerformanceController> logger)
    {
        _logger = logger;
    }

    public IActionResult GetStats()
    {
        Response.Headers.Add("Refresh", "5");

        var processor = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        var memory = new PerformanceCounter("Memory", "Available MBytes");
        
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        var response = new
        { 
            Processor = processor.NextValue(),
            Memory = memory.NextValue(),
            Drives = allDrives.Select(x => new
            {
                x.Name,
                x.DriveType,
                x.VolumeLabel,
                x.DriveFormat,
                x.AvailableFreeSpace,
                x.TotalFreeSpace,
                x.TotalSize
            })
        };

        return this.Ok(response);
    }
}
