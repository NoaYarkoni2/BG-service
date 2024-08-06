namespace BGService.Services
{
    public class FileCheckControlService
    {
        //private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        private FileCheckService _fileCheckService;
        private CancellationTokenSource _cts;

        public FileCheckControlService(IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider)
        {
            //_applicationLifetime = applicationLifetime;
            _serviceProvider = serviceProvider;
        }

        public void Start(string filePath)
        {
            if (_fileCheckService == null || !_fileCheckService.IsRunning)
            {
                _cts = new CancellationTokenSource();
                _fileCheckService = _serviceProvider.GetRequiredService<FileCheckService>();
                _fileCheckService.SetFilePath(filePath);
                Task.Run(() => _fileCheckService.StartAsync(_cts.Token));
            }
        }

        public void Stop()
        {
            if (_fileCheckService != null && _fileCheckService.IsRunning)
            {
                _cts.Cancel();
                _fileCheckService.StopAsync(_cts.Token).Wait();
                _fileCheckService = null;
            }
        }

        public bool IsRunning()
        {
            return _fileCheckService != null && _fileCheckService.IsRunning;
        }
    }
}
