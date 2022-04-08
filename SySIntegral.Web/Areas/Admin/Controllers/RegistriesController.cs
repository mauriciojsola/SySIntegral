using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Repositories;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    [Route("Admin/[Controller]")]
    [Area("Admin")]
    [Authorize]
    public class RegistriesController : Controller
    {
        private readonly IRepository<EggRegistry> _eggRegistryRepository;

        public RegistriesController(IRepository<EggRegistry> eggRegistryRepository)
        {
            _eggRegistryRepository = eggRegistryRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            var users = _eggRegistryRepository.GetAll().Take(200).OrderByDescending(x => x.ReadTimestamp).ToList();
            return View(new RegistriesModel
            {
                Registries = users,
                TotalRecords = _eggRegistryRepository.GetAll().Count()
            });
        }

        public class RegistriesModel
        {
            public RegistriesModel()
            {
                Registries = new List<EggRegistry>();
            }

            public IList<EggRegistry> Registries { get; set; }
            public int TotalRecords { get; set; }
        }
    }
}
