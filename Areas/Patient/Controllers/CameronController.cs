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
using Microsoft.AspNetCore.Authorization;

namespace etherapist.patient.controllers;
[Area("Patient")]
[Authorize(Roles = SD.Role_Patient)]
public class CameronController: Controller {
    private readonly ApplicationDbContext _db;

    public CameronController(ApplicationDbContext db) {
        _db = db;
    }

    public IActionResult Index() {
        List<CameronTest> cameronTests = _db.CameronTests.ToList();
        CameronVM cameronVm;

        if (cameronTests.Count > 2) {
            cameronVm = new() {
                tests = _db.CameronTests.ToList().GetRange(0, 3)
            };
        }

        else {
            cameronVm = new() {
                tests = _db.CameronTests.ToList()
            };
        }

        
        return View(cameronVm);
    }

    public IActionResult Create() {
        CameronVM cameronVm = new() {
            questions = _db.CameronQuestions.ToList()
        };

        return View(cameronVm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CameronVM cameronVm) {
        if(cameronVm.question_2 == null) {
            return View(cameronVm);
        }

        if (cameronVm.question_2 == "None") {
            return View(nameof(Index));
        }

        await _db.CameronTests.AddAsync(
            new CameronTest () {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Result = cameronVm.question_2,
                TestType = SD.testQE,
                CreatedAt = DateTime.Now.ToShortDateString(),
                Status = SD.test_status_ready
            }
        );
        await _db.SaveChangesAsync();

        Int32 testId = _db.CameronTests.Local.ToList()[0].Id;

        // // Grab the Scheduler instance from the Factory
        // StdSchedulerFactory factory = new StdSchedulerFactory();
        // IScheduler scheduler = await factory.GetScheduler();

        // await scheduler.Start();

        // // define the job and tie it to our HelloJob class
        // IJobDetail job = JobBuilder.Create<UpdateTestStatusJob>()
        //     .WithIdentity($"job{testId}", $"group{testId}")
        //     .UsingJobData("testId", testId)
        //     .Build();

        // // Trigger the job to run now, and then repeat every 10 seconds
        // ITrigger trigger = TriggerBuilder.Create()
        //     .WithIdentity($"trigger{testId}", $"group{testId}")
        //     .StartAt(DateTimeOffset.FromUnixTimeSeconds(10))
        //     .Build();

        // await scheduler.ScheduleJob(job, trigger);

        TempData["success"] = "Success, your data is being processed by cameron";
        return RedirectToAction(nameof(History));
    }

    public IActionResult History () {
        List<CameronTest> tests = _db.CameronTests.ToList();
        
        return View(tests);
    }
}

// public class UpdateTestStatusJob : IJob
// {
//     private readonly ApplicationDbContext _db;

//     public UpdateTestStatusJob (ApplicationDbContext db) {
//         _db = db;
//     }
//     public async Task Execute(IJobExecutionContext context)
//     {
//         Console.WriteLine("Job is being executed");
//         Int32 testId = context.JobDetail.JobDataMap.GetInt("testId");
//         CameronTest? test = await _db.CameronTests.FindAsync(testId);
//         if (test == null) {
//             return;
//         }

//         test.Status = SD.test_status_ready;
//         await _db.SaveChangesAsync();

//         return;
//     }
// }
