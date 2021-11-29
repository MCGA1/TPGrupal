using CintaApi.Interfaces;
using CintaApi.Models;
using CintaApi.Services;
using CommonServices.Context;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CintaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CintaController : ControllerBase
    {
        public CintaController()
        {

        }

        [HttpPost("PonerBulto")]
        public async Task Poner(IEnumerable<Bulto> bulto)
        {

            if (bulto==null)
            {
                throw new Exception();
            }

            await ServiceBusMessageService.PonerBulto(bulto);
        }


    


        [HttpPost("speeed/{speed}")]
        public async Task ConfigureSpeed([FromRoute] int speed)
        {
            await ServiceBusMessageService.SetVelocity(speed).ConfigureAwait(false);
        }

        [HttpPost("InitializeProcess")]
        public  Task InitializeProcess()
        {
            ServiceBusMessageService.IntitializeProcess();
            return Task.CompletedTask;
        }


        [HttpPost("StopCintaProcess")]
        public Task StopCintaProcess()
        {
            ServiceBusMessageService.StopProcess();
            return Task.CompletedTask;
        }

     

        [HttpPost]
        public object Configuration(APIConfiguration config)
        {
            ServiceBusMessageService.SetVelocity(config.TiempoDeProcesamiento);
            ServiceBusMessageService.SetStatus(config.Estado);
            Log.Information($"\nMensaje de configuracion recibido, parametros: \n- Tiempo de procesamiento: {config.TiempoDeProcesamiento} \n- Estado: {config.Estado}");
            return HttpStatusCode.OK;
        }

        [HttpGet("apiconfig/{status}")]
        public object Status(ServiceStatus status)
        {
            ServiceBusMessageService.SetStatus(status);
            Log.Information($"Mensaje de estado recibido: {status}.");
            return HttpStatusCode.OK;
        }

        [HttpGet("stats")]
        public object Status()
        {
            Log.Information($"Mensaje de estado respondido.");
            return HttpStatusCode.OK;
        }
    }
}
