using Microsoft.AspNetCore.Mvc;
using XCoreAssignment.Services;

namespace XCoreAssignment.Controllers;

public class QRController : Controller
{
    private readonly IQRService _qrService;

    public QRController(IQRService qrService)
    {
        _qrService = qrService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var viewResult = _qrService.IndexGet();
        return View(viewResult.View, viewResult.Model);
    }

    [HttpPost]
    public IActionResult Index(IFormFile FormFile)
    {
        var viewResult = _qrService.IndexPost(FormFile);
        return View(viewResult.View, viewResult.Model);
    }
}