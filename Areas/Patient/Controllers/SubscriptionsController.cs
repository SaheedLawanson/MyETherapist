using System.Security.Claims;
using etherapist.Data;
using etherapist.Models;
using etherapist.Models.ViewModels;
using etherapist.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace etherapist.patient.controllers;
[Area("Patient")]
[Authorize(Roles = SD.Role_Patient)]
public class SubscriptionsController: Controller {
    private readonly ApplicationDbContext _db;

    public SubscriptionsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index() {
        String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<PatientSubscription> pSubs = _db.PatientSubscriptions
            .Where(pSub => pSub.PatientId == userId && pSub.PaymentStatus == SD.subPaytStatus_success)
            .Include("Subscription").ToList();

        PatientSubscription activeSub;
        if (pSubs.Count == 0) {
            activeSub = null;
        }
        else {
            activeSub = pSubs.FirstOrDefault(sub => sub.Status == SD.subscription_active);
        }

        SubscriptionVM subscriptionVm = new() {
            PSubscriptions = pSubs,
            ActiveSubscription = activeSub,
            Subscriptions = _db.Subscriptions.ToList()
        };

        return View(subscriptionVm);
    }

    public IActionResult Create() {
        SubscriptionVM subscriptionVm = new () {
            Subscriptions = _db.Subscriptions.ToList()
        };
        return View(subscriptionVm);
    }

    [HttpPost]
    public IActionResult Create(Int32 myId) {
        String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        IEnumerable<Subscription> subs = _db.Subscriptions;
        Subscription sub = subs.First(sub => sub.Id == myId);

        // Create Subscription View model
        SubscriptionVM subscriptionVm = new () {
            PSubscription = new () {
                PatientId = userId,
                Status = SD.subscription_active,
                SubscriptionId = myId,
                SessionsLeft = sub.AvailableSessions,
                PaymentStatus = SD.subPaytStatus_pending
            },
            Subscriptions = subs.ToList()
        };

        // Check if patient has an active subscription
        PatientSubscription? activeSub = _db.PatientSubscriptions
            .FirstOrDefault(pSub => pSub.Status == SD.subscription_active && pSub.PatientId == userId);
        
        if (activeSub != null) {
            TempData["error"] = "You currently have an active subscription";
            return View(subscriptionVm);
        }

        _db.PatientSubscriptions.Add(subscriptionVm.PSubscription);
        _db.SaveChanges();
        

        // Stripe payment
        var domain = "https://localhost:7126";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = sub.Price,
                        Currency = "ngn",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = sub.PlanName,
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = $"{domain}/Patient/Subscriptions/SubscriptionConfirmation?pSubId={subscriptionVm.PSubscription.Id}",
            CancelUrl = $"{domain}/Patient/Subscriptions/Index",
        };

        var service = new SessionService();
        Stripe.Checkout.Session session = service.Create(options);

        subscriptionVm.PSubscription.PaymentSessionId = session.Id;
        subscriptionVm.PSubscription.PaymentIntentId = session.PaymentIntentId;
        _db.SaveChanges();

        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);

        // TempData["success"] = $"Successfully subscribed to the {sub.PlanName}";
        // return RedirectToAction("Index");
    }

    public IActionResult SubscriptionConfirmation(Int32 pSubId) {
        PatientSubscription pSub = _db.PatientSubscriptions.Find(pSubId);
        var Service = new SessionService();
        Stripe.Checkout.Session paymentSession = Service.Get(pSub.PaymentSessionId);

        if (paymentSession.PaymentStatus == "paid") {
            pSub.PaymentStatus = SD.subPaytStatus_success;
            _db.SaveChanges();
        }

        TempData["success"] = $"Successfully created subscription";
        return RedirectToAction("Index");
    }
}