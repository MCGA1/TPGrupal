using Brazo.Core.Contracts;
using Brazo.Core.Management;
using CommonDomain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brazo.Core.Jobs
{
	public class BrazoJob : JobService
	{
		public override string Name => "Servicio de procesamiento del Brazo";

		ILogger<BrazoJob> _logger;
		IBrazoManagement _brazoService;
		ICintaManagement _cintaService;
		IPrensaManagement _prensaService;

		public BrazoJob(ILogger<BrazoJob> logger, IBrazoManagement brazoService, ICintaManagement cintaService, IPrensaManagement prensaService) : base(logger)
		{
			_logger = logger;
			_brazoService = brazoService;
			_cintaService = cintaService;
			_prensaService = prensaService;
		}

		public override async Task RunAsync(CancellationToken stoppingToken)
		{
			try
			{
				_logger.LogInformation("Starting brazo package proces");

				while (!stoppingToken.IsCancellationRequested)
				{
					await ProcessPackage();
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error processing package");
			}
		}

		private async Task ProcessPackage()
		{
			var config = await _brazoService.GetConfiguration();

			try
			{
				_logger.LogInformation("Checking data from MQ");

				//read mq (cinta)
				var bulto = await _cintaService.GetPackage();

				if (bulto == null)
				{
					await Task.Delay(2000); //safety sleep
					return;
				}

				//process data(opening) (sleep)
				_logger.LogInformation($"Processing package. Waiting [{config.TiempoDeProcesamiento}] ms");
				await Task.Delay(config.TiempoDeProcesamiento);

				await SaveData(bulto);
				await _prensaService.SendPackage(bulto);
				await _cintaService.ProcessPackage();
				
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error processing package");
			}

		}

		private async Task SaveData(Bulto package)
		{
			/*
			string sqlOrderDetails = "SELECT TOP (1000) * FROM[MCGA.TpGrupal].[EventLogging].[Logs] ORDER BY Timestamp desc";

			using var connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

			var list = connection.Query<Bulto>(sqlOrderDetails).ToList();

			return list;
			*/
		}
	}
}
