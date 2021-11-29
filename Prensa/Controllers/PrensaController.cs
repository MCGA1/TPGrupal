using CommonServices.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Prensa.PrensaSystem;
using CommonServices.Entities.Enum;
using Serilog;

namespace Prensa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrensaController : ControllerBase
    {

        private readonly ILogger<PrensaController> _logger;

        public PrensaController(ILogger<PrensaController> logger)
        {
            _logger = logger;
        }

        [HttpGet("configuration")]
        public object Configuration()
        {
            return new APIConfiguration()
            {
                Estado = PrensaWorker.State ? ServiceStatus.Running : ServiceStatus.Stopped,
                TiempoDeProcesamiento = MaquinaPrensado.CurrentSpeed(),
                Sensores = new SensorConfiguration[] { new SensorConfiguration { Nombre = "Sensor pasivo", Estado = SensoresSystem.SensorPasivo.IsPaused() ? ServiceStatus.Stopped : ServiceStatus.Running } }
            };
        }


        [HttpPut("configuration")]
        public object Configuration(APIConfiguration config)
        {
            MaquinaPrensado.SetSpeed(config.TiempoDeProcesamiento);
            PrensaWorker.SetStatus(config.Estado);
            var item = config.Sensores.FirstOrDefault().Estado;
            SensoresSystem.SensorPasivo.SetPaused(item == ServiceStatus.Running ? true : false);
            Log.Information($"\nMensaje de configuracion recibido, parametros: \n- Tiempo de procesamiento: {config.TiempoDeProcesamiento} \n- Estado: {config.Estado} \n- Sensores: {config.Sensores.FirstOrDefault().Estado}");
            
            return HttpStatusCode.OK;
        }

        [HttpPost("status/{type}")]
        public object Status(ServiceStatus type)
        {
            PrensaWorker.SetStatus(type);
            Log.Information($"Mensaje de estado recibido: {type}.");
            return HttpStatusCode.OK;
        }

        [HttpGet("status")]
        public object Status()
        {
            Log.Information($"Mensaje de estado respondido.");
            return HttpStatusCode.OK;
        }
    }
}
