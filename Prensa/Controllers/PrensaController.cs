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

        [HttpGet]
        public void Get()
        {

        }

        [HttpGet]
        public object Configuration(APIConfiguration config)
        {
            MaquinaPrensado.SetSpeed(config.TiempoDeProcesamiento);
            PrensaWorker.SetStatus(config.Estado);
            SensorActivo.

            return HttpStatusCode.OK;
        }




    }
}
