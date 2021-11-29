using CintaApi.Interfaces;
using CintaApi.Models;
using CintaApi.Services;
using CommonDomain;
using CommonServices;
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
        public async Task Poner(IEnumerable<Models.Bulto> bulto)
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


        [HttpGet("status/{status}")]
        public object Status(ServiceStatus status)
        {
            ServiceBusMessageService.SetStatus(status);
            Log.Information($"Mensaje de estado recibido: {status}.");
            return HttpStatusCode.OK;
        }

        [HttpGet("status")]
        public object Status()
        {
            Log.Information($"Mensaje de estado respondido.");
            return HttpStatusCode.OK;
        }


        [HttpGet("configuration")]
        public object Configuration()
        {
            return new APIConfiguration()
            {
                Estado = ServiceBusMessageService._paused ? ServiceStatus.Stopped : ServiceStatus.Running,
                TiempoDeProcesamiento = ServiceBusMessageService.CurrentSpeed,
                Sensores = null 
            };
        }


        [HttpPut("configuration")]
        public object Configuration(APIConfiguration config)
        {
            ServiceBusMessageService.SetVelocity(config.TiempoDeProcesamiento);
            ServiceBusMessageService.SetStatus(config.Estado);
            Log.Information($"\nMensaje de configuracion recibido, parametros: \n- Tiempo de procesamiento: {config.TiempoDeProcesamiento} \n- Estado: {config.Estado}");
            return HttpStatusCode.OK;
        }

        //[HttpPut("status/{type}")]
        //public object status(ServiceStatus type)
        //{
        //    prensaworker.setstatus(type);
        //    log.information($"mensaje de estado recibido: {type}.");
        //    return httpstatuscode.ok;
        //}


        [HttpGet("packages")]
        public object Packages()
        {
            List<PackageItem> packages = CommonServices.BultoManagementFactory.GetBultoIngresadoManager().GetDateTimes();
            return packages;
        }


    }
}
