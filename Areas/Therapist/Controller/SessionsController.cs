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
public class SessionsController: Controller {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public SessionsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager ) {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index(String status) {
        String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        List<Session> sessions;

        if (status == "processing") {
            sessions = _db.Sessions.Where(
                session => session.TherapistId == userId
                && session.Status <= SD.session_awaitingTherapistApproval
                ).Include("Patient").ToList();
        }
        else if (status == "pending") {
            sessions = _db.Sessions.Where(
                session => session.TherapistId == userId
                && session.Status == SD.session_completed
                ).Include("Patient").ToList();
        }
        else if (status == "completed") {
            sessions = _db.Sessions.Where(
                session => session.TherapistId == userId
                && session.Status >= SD.session_sessionCompleted
                ).Include("Patient").ToList();
        }
        else {
            sessions = _db.Sessions.Where(
                session => session.TherapistId == userId
                ).Include("Patient").ToList();
        }

        // List<Session> sessions = _db.Sessions
        //     .Where(session => session.TherapistId == userId)
        //     .Include("Patient")
        //     .ToList();

        return View(sessions);
    }

    public IActionResult Details(Int32 sessionId) {
        String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Session? patientSession = _db.Sessions.Where(sesh => sesh.TherapistId == userId)
            .Include("Patient").Include("Therapist")
            .FirstOrDefault(session => session.Id == sessionId);

        if (patientSession == null) {
            TempData["error"] = "This session does not exist";
            return RedirectToAction(nameof(Index));
        }

        return View(patientSession);
    }

    [HttpPost]
    public IActionResult Details(Session session) {
        String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Session? patientSession = _db.Sessions.Where(sesh => sesh.TherapistId == userId)
            .Include("Patient").Include("Therapist")
            .FirstOrDefault(sesh => sesh.Id == session.Id);

        if (patientSession.Status == SD.session_awaitingTherapistApproval) {
                patientSession.Status = SD.session_completed;
                _db.SaveChanges();
    }

        else {
            TempData["error"] = "Changes not permitted";
            return View(patientSession);
        }

        TempData["success"] = "Session has been approved";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Complete (Session session) {
        String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Session? patientSession = _db.Sessions.Where(sesh => sesh.TherapistId == userId)
            .Include("Patient").Include("Therapist")
            .FirstOrDefault(sesh => sesh.Id == session.Id);

        if (session.StartTime == DateTime.Now.ToShortDateString() || session.StopTime == DateTime.Now.ToShortDateString()) {
            TempData["error"] = "Please fill in the start time or stop time";
            return View(nameof(Details), patientSession);
        }

        if (session.StartTime == null) {
            TempData["error"] = "Please fill in the start time";
            return View(nameof(Details), patientSession);
        }

        if (session.StopTime == null) {
            TempData["error"] = "Please fill in the stop time";
            return View(nameof(Details), patientSession);
        }

        patientSession.StartTime = session.StartTime;
        patientSession.StopTime = session.StopTime;
        patientSession.Status = SD.session_sessionCompleted;
        _db.SaveChanges();

        TempData["success"] = "Session lifecycle has been completed"; 
        return RedirectToAction(nameof(Index));
    }
}