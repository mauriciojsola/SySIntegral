using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.CheckPoints;
using SySIntegral.Core.Repositories.Reports;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    [Route("Admin/[Controller]")]
    [Area("Admin")]
    [Authorize]
    public partial class ReportsController : SySIntegralBaseController
    {
        private readonly IRepository<EggRegistry> _eggRegistryRepository;
        private readonly ICheckPointRepository _checkPointRepository;
        private readonly ICheckPointCountingsReportRepository _checkPointCountingsReportRepository;

        public ReportsController(IRepository<EggRegistry> eggRegistryRepository,
            ICheckPointRepository checkPointRepository,
            ICheckPointCountingsReportRepository checkPointCountingsReportRepository)
        {
            _eggRegistryRepository = eggRegistryRepository;
            _checkPointRepository = checkPointRepository;
            _checkPointCountingsReportRepository = checkPointCountingsReportRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

      
    }
}
