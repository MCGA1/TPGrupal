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
    [Route("[controller]")]
    public class PrensaController : ControllerBase
    {

        private readonly ILogger<PrensaController> _logger;

        public PrensaController(ILogger<PrensaController> logger)
        {
            _logger = logger;
        }




        [HttpPost]
        [Route("configuration")]
        public object Configuration(APIConfiguration config)
        {
            MaquinaPrensado.SetSpeed(config.TiempoDeProcesamiento);
            PrensaWorker.SetStatus(config.Estado);
            var item = config.Sensores.FirstOrDefault().Estado;
            SensoresSystem.SensorPasivo.SetPaused(item == ServiceStatus.Running ? true : false);
            Log.Information($"\nMensaje de configuracion recibido, parametros: \n- Tiempo de procesamiento: {config.TiempoDeProcesamiento} \n- Estado: {config.Estado} \n- Sensores: {config.Sensores.FirstOrDefault().Estado}");
            return HttpStatusCode.OK;
        }

        [HttpPost]
        [Route("Status/{status}")]
        public object Status(ServiceStatus status)
        {
            PrensaWorker.SetStatus(status);
            Log.Information($"Mensaje de estado recibido: {status}.");
            return HttpStatusCode.OK;
        }

        [HttpGet]
        [Route("Status")]
        public object Status()
        {
            Log.Information($"Mensaje de estado respondido.");
            return HttpStatusCode.OK;
        }
    }
}
