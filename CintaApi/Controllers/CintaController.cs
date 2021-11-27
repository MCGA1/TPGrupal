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
        public CintaController()
        {

        }

        // GET: api/<CintaController>
        [HttpPost("PonerBulto")]
        public async Task Poner([FromBody] IEnumerable<Bulto> bulto)
        {

            if (bulto==null)
            {
                throw new Exception();
            }

            await ServiceBusMessageService.PonerBulto(bulto);

        }

        [HttpGet("individual/{bultoId}")]
        public async Task<Bulto> GetIndividual([FromRoute] string bultoId)
        {
            return await ServiceBusMessageService.GetIndididualBulto(bultoId);
        }

        [HttpGet("Health")]
        public async Task<List<string>> Health()
        {
            return await ServiceBusMessageService.CheckQueueMensagges().ConfigureAwait(false);
        }


        [HttpPost("speeed/{speed}")]
        public async Task ConfigureSpeed([FromRoute] int speed)
        {
            await ServiceBusMessageService.SetVelocity(speed).ConfigureAwait(false);
        }

    }
}