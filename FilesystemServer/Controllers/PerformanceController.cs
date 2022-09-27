using System.Diagnostics;
using System.Runtime.InteropServices;
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
        const string NotSupported = "NotSupported";

        Response.Headers.Add("Refresh", "5");

        object? processor = null;
        object? memory = null;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            processor = new PerformanceCounter("Processor", "% Processor Time", "_Total").NextValue();
            memory = new PerformanceCounter("Memory", "Available MBytes").NextValue();
        }
        
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        var response = new
        { 
            Processor = processor ?? NotSupported,
            Memory = memory ?? NotSupported,
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
