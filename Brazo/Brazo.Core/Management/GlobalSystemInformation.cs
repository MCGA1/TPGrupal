using CommonServices.Entities;
using CommonServices.Entities.Enum;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brazo.Core.Management
{
	public class GlobalSystemInformation
	{
		public APIConfiguration Config { get; set; }

		public ServiceStatus Status { get; set; }

		public GlobalSystemInformation(IConfiguration config)
		{
			Config = new APIConfiguration() { Estado = ServiceStatus.Running};
			config.GetSection("APIConfiguration").Bind(Config);

			Status = ServiceStatus.Running;
		}
	}
}
