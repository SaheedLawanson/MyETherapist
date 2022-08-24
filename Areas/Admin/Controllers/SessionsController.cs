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

namespace etherapist.admin.controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]

public class SessionsController: Controller {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public SessionsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager ) {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index(String status) {
        List<Session> sessions;

        if (status == "processing") {
            sessions = _db.Sessions.Where(
                session => session.Status <= SD.session_awaitingTherapistAssign
                ).Include("Patient").Include("Therapist").ToList();
        }
        else if (status == "approved") {
            sessions = _db.Sessions.Where(
                session => session.Status == SD.session_awaitingTherapistApproval
                ).Include("Patient").Include("Therapist").ToList();
        }
        else if (status == "completed") {
            sessions = _db.Sessions.Where(
                session => session.Status >= SD.session_sessionCompleted
                ).Include("Patient").Include("Therapist").ToList();
        }
        else {
            sessions = _db.Sessions.Where(
                session => session.Status >= SD.session_awaitingTherapistAssign
                ).Include("Patient").Include("Therapist").ToList();
        }

        return View(sessions);
    }

    public IActionResult Details(Int32 sessionId) {
        Session? patientSession = _db.Sessions
            .Include("Patient").Include("Therapist")
            .FirstOrDefault(session => session.Id == sessionId);
            
        List<SelectListItem> therapists = _userManager.GetUsersInRoleAsync(SD.Role_Therapist)
            .GetAwaiter().GetResult()
            .Select(therapist => new SelectListItem{Text=$"{therapist.FirstName} {therapist.LastName}", Value=therapist.Id})
            .ToList();

        SessionVM sessionVm = new() {
            session = patientSession,
            therapists = therapists
        };

        return View(sessionVm);
    }

    [HttpPost]
    public IActionResult Details(SessionVM sessionVm) {
        Session? patientSession = _db.Sessions
            .Include("Patient").Include("Therapist")
            .FirstOrDefault(session => session.Id == sessionVm.session.Id);

        if (patientSession.TherapistId != null) {
            sessionVm.session = patientSession;
            TempData["error"] = "A therapist has been assigned to this patient";
            return View(sessionVm);
        }

        patientSession.TherapistId = sessionVm.session.Therapist.Id;
        patientSession.Status = SD.session_awaitingTherapistApproval;
        _db.SaveChanges();

        TempData["success"] = "Therapist successfully assigned";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult History () {
        return View();
    }
}