using Microsoft.AspNetCore.Mvc;
using XCoreAssignment.Services;

namespace XCoreAssignment.Controllers;

public class QRController : Controller
{

    private readonly IQRControllerService _qrControllerService;

    public QRController(IQRControllerService qrControllerService)
    {
        _qrControllerService = qrControllerService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var result = _qrControllerService.IndexGet();
        return View(result.View, result.Model);
    }

    [HttpPost]
    public IActionResult Index(IFormFile FormFile)
    {
        var result = _qrControllerService.IndexPost(FormFile);
        return View(result.View, result.Model);
    }

}