using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brazo.Core
{
	public abstract class JobService : BackgroundService
	{
		public ILogger<JobService> _logger { get; set; }

		public CancellationToken Token { get; set; }

		public abstract string Name { get; }

		public JobService(ILogger<JobService> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var runnnigTask = RunAsync(stoppingToken);
			_logger.LogInformation("Job service [{0}] started.", Name);
			await runnnigTask;
		}


		public override Task StartAsync(CancellationToken cancellationToken)
		{
			Token = cancellationToken;
			_logger.LogInformation("Trying to start job. Name [{0}]", this.Name);
			return base.StartAsync(cancellationToken);

		}

		public override Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Trying to stop job. Name [{0}]", this.Name);
			return base.StopAsync(cancellationToken);
		}

		public abstract Task RunAsync(CancellationToken stoppingToken);
	}
}
