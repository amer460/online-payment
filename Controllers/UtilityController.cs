using Microsoft.AspNetCore.Mvc;
using XCoreAssignment.Models;
using XCoreAssignment.Services;
using XCoreAssignment.ViewModels.Utility;

namespace XCoreAssignment.Controllers;

public class UtilityController : Controller
{
    private readonly IUtilityService _utilityService;

    public UtilityController(IUtilityService utilityService)
    {
        _utilityService = utilityService;
    }

    public IActionResult Template()
    {
        var viewResult = _utilityService.TemplateGet();
        return View(viewResult.View,viewResult.Model);
    }

    [HttpPost]
    [CheckModelState]
    public async Task<IActionResult> Template(UtilityTemplateVM vm)
    {
        var viewResult = await _utilityService.TemplatePostAsync(vm);
        return View(viewResult.View, viewResult.Model);
    }
}
