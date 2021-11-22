using CintaApi.Interfaces;
using CintaApi.Models;
using CintaApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CintaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CintaController : ControllerBase
    {
        private IServiceBusQueueMessage _serviceBusMessageService;
        public CintaController(IServiceBusQueueMessage serviceBusMessageService)
        {
            _serviceBusMessageService = serviceBusMessageService;
        }

        // GET: api/<CintaController>
        [HttpGet("{id}")]
        public async  Task<IEnumerable<Bulto>> Get( [FromRoute] string id)
        {
            return  await _serviceBusMessageService.SacarBulto(id);
        }

        // GET: api/<CintaController>
        [HttpPost("PonerBulto")]
        public void Poner(Bulto bulto)
        {
             _serviceBusMessageService.PonerBulto(bulto);
        }
    }
}