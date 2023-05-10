using Microsoft.AspNetCore.Mvc;

namespace XCoreAssignment.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }
}