using Brazo.Core.Contracts;
using CommonDomain;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Brazo.Core.Management
{
	public class PrensaManagement : IPrensaManagement
	{
		SenderConfig _config;
		private ConnectionFactory _factory;
		private ILogger<PrensaManagement> _logger;

		public PrensaManagement(ILogger<PrensaManagement> logger, IConfiguration conf)
		{
			_config = new();
			conf.GetSection("SenderConfig").Bind(_config);
			_factory = new ConnectionFactory() { HostName = _config.HostName };

			_logger = logger;
		}

		public async Task SendPackage(Bulto package)
		{
			try
			{
				_logger.LogInformation($"Sending package to prensa. Message [{package.Nombre}]");

				using var connection = _factory.CreateConnection();
				using var channel = connection.CreateModel();

				channel.QueueDeclare(queue: _config.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

				var bulto = JsonConvert.SerializeObject(package);
				var body = Encoding.UTF8.GetBytes(bulto);

				channel.BasicPublish(exchange: "", routingKey: _config.QueueName, basicProperties: null, body: body);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error processing data to be sender");
				throw;
			}
		}

	}
}
