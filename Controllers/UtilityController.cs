using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Text.Json;
using XCoreAssignment.Models;
using XCoreAssignment.Services;
using XCoreAssignment.ViewModels.Utility;

namespace XCoreAssignment.Controllers
{
    public class UtilityController : Controller
    {
        private readonly IUtilityService _utilityService;

        public UtilityController(IUtilityService utilityService)
        {
            _utilityService = utilityService;
        }

        public IActionResult Template()
        {
            var viewResult = _utilityService.Template();
            return View(viewResult.View,viewResult.Model);
        }

        [HttpPost]
        [CheckModelState]
        public async Task<IActionResult> Template(UtilityTemplateVM vm)
        {
            var viewResult = await _utilityService.PayAsync(vm);
            return View(viewResult.View, viewResult.Model);
        }
    }
}
