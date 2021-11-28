using Brazo.Core.Contracts;
using Brazo.Core.Management;
using Brazo.Core.Model;
using CommonDomain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brazo.Core.Jobs
{
	public class BrazoJob : JobService
	{
		public override string Name => "Servicio de procesamiento del Brazo";

		IBrazoManagement _brazoService;
		IPrensaManagement _prensaService;
		IDataAccessService _dataAccess;

		JobConfig _config;
		ConnectionFactory _factory;

		public BrazoJob(ILogger<BrazoJob> logger, IConfiguration config, IBrazoManagement brazoService, IPrensaManagement prensaService, IDataAccessService dataAccess) : base(logger)
		{
			_brazoService = brazoService;
			_prensaService = prensaService;
			_dataAccess = dataAccess;

			_config = new();
			config.GetSection("JobConfig").Bind(_config);

			_factory = new ConnectionFactory() { HostName = _config.HostName };
			
		}

		public override async Task RunAsync(CancellationToken stoppingToken)
		{
			try
			{
				_logger.LogInformation("Starting brazo package proces");
				using var connection = _factory.CreateConnection();
				using var channel = connection.CreateModel();

				while (!stoppingToken.IsCancellationRequested)
				{
					await ProcessPackage(connection, channel);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error processing package");
			}
		}

		private async Task ProcessPackage(IConnection connection, IModel channel)
		{
			var config = await _brazoService.GetConfiguration();

			try
			{
				if (config.Estado == CommonServices.Entities.Enum.ServiceStatus.Stopped)
				{
					_logger.LogInformation("The Brazo service is stopped");
					await Task.Delay(2000);

					return;
				}

				_logger.LogInformation("Checking data from MQ");

				channel.QueueDeclare(queue: _config.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

				if (channel.MessageCount(_config.QueueName) == 0)
				{
					await Task.Delay(2000);
					_logger.LogInformation("The Queue is empty safety sleep");
					return;
				}
				BasicGetResult result;
				Bulto bulto;
				(bulto, result) = await GetBulto(channel);

				_logger.LogInformation($"new package to be processed. Name[{bulto.Nombre}] ");

				_logger.LogInformation($"Processing package. Waiting [{config.TiempoDeProcesamiento}] ms");
				await Task.Delay(config.TiempoDeProcesamiento);

				await _prensaService.SendPackage(bulto);
				await _dataAccess.SavePackage(bulto);

				channel.BasicAck(result.DeliveryTag, false);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error processing package");
			}

		}

		private async Task<(Bulto bulto, BasicGetResult result)> GetBulto(IModel channel)
		{
			var result = channel.BasicGet(_config.QueueName, false);
			ReadOnlyMemory<byte> body = result.Body;

			var json = Encoding.UTF8.GetString(body.ToArray());
			var bulto = JsonConvert.DeserializeObject<Bulto>(json);

			return (bulto, result);
		}

	}
}
