using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using etherapist.Models;

namespace etherapist.Controllers;
[Area("Home")]

public class CareersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
