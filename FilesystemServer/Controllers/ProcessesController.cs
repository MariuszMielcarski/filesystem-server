using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FilesystemServer.Model;

namespace FilesystemServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ProcessesController : ControllerBase
{
    private readonly ILogger<FilesystemController> _logger;

    public ProcessesController(ILogger<FilesystemController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    public IEnumerable<ProcessDto> Get()
    {
        this._logger.LogInformation($"Somebody is asking for processes.");

        var processes = Process.GetProcesses();
        var result = processes.Select(x => new ProcessDto { Id = x.Id, Name = x.ProcessName });
        
        return result;
    }

    [HttpDelete("Process")]
    public void DeleteProcess(int id)
    {
        this._logger.LogInformation($"Somebody is killing the process with ID {id}.");

        var process = Process.GetProcessById(id);
        process.Kill();
    }

    [HttpPost("Process")]
    public ProcessDto PostProcess(string command)
    {
        this._logger.LogInformation($"Somebody is starting the process with the command: {command}.");

        var process = Process.Start(command);
        return new ProcessDto { Id = process.Id, Name = process.ProcessName };
    }
}
