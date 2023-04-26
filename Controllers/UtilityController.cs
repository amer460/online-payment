using Microsoft.AspNetCore.Mvc;
using XCoreAssignment.ViewModels.Utility;

namespace XCoreAssignment.Controllers
{
    public class UtilityController : Controller
    {
        [HttpGet]
        public IActionResult Template(UtilityTemplateVM? vm = null)
        {
            vm ??= new();

            return View(vm);
        }
    }
}
