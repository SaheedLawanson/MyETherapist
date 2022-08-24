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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace etherapist.therapist.controllers;
[Area("Therapist")]
[Authorize(Roles = SD.Role_Therapist)]
public class CalendarController: Controller {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public CalendarController(ApplicationDbContext db, UserManager<ApplicationUser> userManager ) {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index() {
        String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        CalendarVM calendarVm = new();
        List<String> dates = _db.Sessions
            .Where(session => session.TherapistId == userId 
                && session.Status < SD.session_sessionCompleted)
            .Select(session => session.ScheduledTime.ToString("yyyy/MM/dd"))
            .ToList();

        if (dates.Count != 0) {
            calendarVm.presets += dates[0];
            foreach (String date in dates.GetRange(1, dates.Count -1)) {
                calendarVm.presets += date;
            }
        }

        return View(calendarVm);
    }
}