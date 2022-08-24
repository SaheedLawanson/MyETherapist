using System.Security.Claims;
using etherapist.Data;
using etherapist.Models;
using etherapist.Models.ViewModels;
using etherapist.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

using Quartz;
using Quartz.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace etherapist.admin.controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]

public class TherapistsController: Controller {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TherapistsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager) {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index() {
        List<ApplicationUser> therapists = _userManager.GetUsersInRoleAsync(SD.Role_Therapist)
            .GetAwaiter().GetResult().ToList();
        
        return View(therapists);
    }

    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Int32 id) {
        return View();
    }

    public IActionResult History () {
        return View();
    }
}