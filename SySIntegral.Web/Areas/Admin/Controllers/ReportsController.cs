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
    public class ReportsController : Controller
    {
        private readonly IRepository<EggRegistry> _eggRegistryRepository;

        public ReportsController(IRepository<EggRegistry> eggRegistryRepository)
        {
            _eggRegistryRepository = eggRegistryRepository;

        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

      
    }
}
