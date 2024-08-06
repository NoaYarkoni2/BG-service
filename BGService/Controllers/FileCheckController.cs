using BGService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BGService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileCheckController : ControllerBase
    {
        private readonly FileCheckControlService _fileCheckControlService;

        public FileCheckController(FileCheckControlService fileCheckControlService)
        {
            _fileCheckControlService = fileCheckControlService;
        }

        [HttpPost("start")]
        public IActionResult Start([FromBody] string filePath)
        {
            _fileCheckControlService.Start(filePath);
            return Ok($"File check service started for {filePath}.");
        }

        [HttpPost("stop")]
        public IActionResult Stop()
        {
            _fileCheckControlService.Stop();
            return Ok("File check service stopped.");
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            var isRunning = _fileCheckControlService.IsRunning();
            return Ok(new { isRunning });
        }
    }
    }
