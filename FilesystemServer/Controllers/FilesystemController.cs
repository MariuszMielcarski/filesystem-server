using Microsoft.AspNetCore.Mvc;

namespace FilesystemServer.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesystemController : ControllerBase
{
    private readonly ILogger<FilesystemController> _logger;

    public FilesystemController(ILogger<FilesystemController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    public IEnumerable<string> Get(string path)
    {
        this._logger.LogInformation($"Somebody is asking for {path} directory.");

        var directories = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);
        var result = directories.Concat(files);
        return result;
    }

    [HttpGet("File")]
    public string GetFile(string path)
    {
        this._logger.LogInformation($"Somebody is getting for {path} file.");

        var body = System.IO.File.ReadAllText(path);
        return body;
    }

    [HttpPut("File")]
    public void PutFile(string path, [FromBody] string text)
    {
        this._logger.LogInformation($"Somebody is editting {path} file.");

        System.IO.File.WriteAllText(path, text);
    }
}
