using System.Globalization;
using System.Linq;
using System.Security.Claims;
using etherapist.Data;
using etherapist.Models;
using etherapist.Models.ViewModels;
using etherapist.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace etherapist.patient.controllers;
[Area("Patient")]
[Authorize(Roles = SD.Role_Patient)]
public class SessionsController: Controller {
    private readonly ApplicationDbContext _db;

    public SessionsController(ApplicationDbContext db) {
        _db = db;
    }

    public IActionResult Index(String status) {
        String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        List<Session> sessions;

        if (status == "processing") {
            sessions = _db.Sessions.Where(
                session => session.PatientId == userId 
                && session.Status <= SD.session_awaitingTherapistApproval
                ).Include("Subscription").ToList();
        }
        else if (status == "approved") {
            sessions = _db.Sessions.Where(
                session => session.PatientId == userId 
                && session.Status == SD.session_completed
                ).Include("Subscription").ToList();
        }
        else if (status == "completed") {
            sessions = _db.Sessions.Where(
                session => session.PatientId == userId && session.Status >= SD.session_sessionCompleted
                ).Include("Subscription").ToList();
        }
        else {
            sessions = _db.Sessions.Where(
                session => session.PatientId == userId && session.Status >= SD.session_awaitingTherapistAssign
                ).Include("Subscription").ToList();
        }

        SessionVM sessionVm = new () {
            sessions = sessions
        };

        return View(sessionVm);
    }
    public IActionResult Create() {
        SessionVM sessionVm = new SessionVM();
        return View(sessionVm);
    }

    [HttpPost]
    public IActionResult Create(SessionVM sessionVm) {
        // Get User Id
        String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Check if a valid subscription exists for the user
        PatientSubscription? activeSub = _db.PatientSubscriptions
            .FirstOrDefault(pSub => pSub.Status == SD.subscription_active 
            && pSub.PatientId == userId && pSub.PaymentStatus == SD.subPaytStatus_success);
        if (activeSub == null) {
            TempData["error"] = "You currently have no active subscription";
            return View(sessionVm);
        }
        Subscription sub = _db.Subscriptions.Find(activeSub.SubscriptionId);
        
        // Attach Necessary properties
        sessionVm.session.ScheduledTime = DateTime.ParseExact(
            $"{sessionVm.Date} {sessionVm.Time}", 
            "yyyy-M-dd H:m:s", 
            new CultureInfo("en-US")
        );
        sessionVm.session.PatientId = userId;
        sessionVm.session.MeetingSpan = new TimeSpan(0, 30, 0);
        sessionVm.session.MeetingType = $"{activeSub.SessionsLeft} of {sub.AvailableSessions}";
        sessionVm.session.Status = SD.session_awaitingTherapistAssign;
        sessionVm.session.PatientSubscriptionId = activeSub.Id;
        sessionVm.session.SubscriptionId = activeSub.SubscriptionId;

        _db.Sessions.Add(sessionVm.session);

        // Update Patient Subscription
        activeSub.SessionsLeft -= 1;
        if (activeSub.SessionsLeft == 0) {
            activeSub.Status = SD.subscription_inactive;
        }
        _db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details (Int32 myId) {
        SessionVM sessionVm = new() {
            session = _db.Sessions.Include("Therapist").FirstOrDefault(session => session.Id == myId)
        };
        
        return View(sessionVm);
    }

    public IActionResult Rate(SessionVM sessionVm) {
        String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Session? patientSession = _db.Sessions
            .Include("Subscription").Include("Therapist")
            .FirstOrDefault(sesh => sesh.PatientId == userId
                && sesh.Id == sessionVm.session.Id);

        if (patientSession == null) {
            TempData["error"] = "Session not found";
            return View("Details", sessionVm);
        }

        patientSession.Comment = sessionVm.session.Comment;

        if (sessionVm.session.Rating == 0) {
            TempData["error"] = "You can't rate below 1 star";
            sessionVm.session = patientSession;
            return View("Details", sessionVm);
        }

        patientSession.Rating = sessionVm.session.Rating;
        patientSession.Status = SD.session_rated;
        _db.SaveChanges();

        TempData["success"] = "Success applied comment";
        return RedirectToAction(nameof(Index));
    }
}