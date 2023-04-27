using Application.Common.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using XCoreAssignment.Services;
using XCoreAssignment.ViewModels.Utility;

namespace XCoreAssignment.Controllers;

public class UtilityController : Controller
{
    private readonly IUtilityControllerService _utilityService;

    public UtilityController(IUtilityControllerService utilityService)
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
