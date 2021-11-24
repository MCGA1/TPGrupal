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
	public class GloblaSystemInformation
	{
		public APIConfiguration Config { get; set; }

		public ServiceStatus Status { get; set; }

		public GloblaSystemInformation(IConfiguration config)
		{
			Config = new APIConfiguration();
			config.GetSection("APIConfiguration").Bind(Config);

			Status = ServiceStatus.Running;
		}
	}
}
