using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using etherapist.Models;

namespace etherapist.Controllers;

public class AboutUsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
