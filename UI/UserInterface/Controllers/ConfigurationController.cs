using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class ConfigurationController : Controller
    {
        private APIGatewayService _service;

        public ConfigurationController(APIGatewayService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var modelList = new List<ModelConfiguration>();

            var itemsCinta = (await _service.ServicesAllAsync(ServiceType.Cinta)).Where(x => x.Status != ServiceStatus.Failed);
            modelList.Add(new ModelConfiguration() { Type = ServiceType.Cinta, Items = itemsCinta.Select(x => new ModelConfigurationItem() { Name = x.Name, Config = _service.ConfigurationAsync(ServiceType.Cinta, x.Name).GetAwaiter().GetResult() }).ToList() });

            var itemsBrazo = (await _service.ServicesAllAsync(ServiceType.Brazo)).Where(x => x.Status != ServiceStatus.Failed);
            modelList.Add(new ModelConfiguration() { Type = ServiceType.Brazo, Items = itemsBrazo.Select(x => new ModelConfigurationItem() { Name = x.Name, Config = _service.ConfigurationAsync(ServiceType.Brazo, x.Name).GetAwaiter().GetResult() }).ToList() });

            var itemsPrensa = (await _service.ServicesAllAsync(ServiceType.Prensa)).Where(x => x.Status != ServiceStatus.Failed);
            modelList.Add(new ModelConfiguration() { Type = ServiceType.Prensa, Items = itemsPrensa.Select(x => new ModelConfigurationItem() { Name = x.Name, Config = _service.ConfigurationAsync(ServiceType.Prensa, x.Name).GetAwaiter().GetResult() }).ToList() });

            return View(modelList);
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<ModelConfiguration> models)
        {
            foreach (var model in models.Where(x => x.Items != null))
            {
                foreach (var item in model.Items)
                {
                    await _service.Configuration2Async(model.Type, item.Name, item.Config);
                }
            }
            return await Index();
        }

    }
}
