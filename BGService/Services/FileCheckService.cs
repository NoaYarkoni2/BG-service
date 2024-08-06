using System.Data;

namespace BGService.Services
{
    public class FileCheckService : BackgroundService
    {
        private readonly ILogger<FileCheckService> _logger;
        private bool _isRunning;
        private string _filePath;
        private Timer _timer;

        public FileCheckService(ILogger<FileCheckService> logger)
        {
            _logger = logger;         
        }
        public bool IsRunning => _isRunning;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            stoppingToken.Register(() => StopTimer());
            _isRunning = true;
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                _logger.LogInformation("File path not set.");
                return;
            }

            var fileExists = File.Exists(_filePath);
            _logger.LogInformation($"Checking {_filePath}: File exists: {fileExists}");
            _isRunning = true;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            StopTimer();
            return base.StopAsync(cancellationToken);
        }

        private void StopTimer()
        {
            _timer?.Change(Timeout.Infinite, 0);
            _isRunning = false;
        }


        public void SetFilePath(string filePath)
        {
            _filePath = filePath;
        }
    }
}
