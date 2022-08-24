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

namespace etherapist.admin.controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CameronController: Controller {
    private readonly ApplicationDbContext _db;

    public CameronController(ApplicationDbContext db) {
        _db = db;
    }

    public IActionResult Index(String testType) {
        List<CameronTest> tests;

        if (testType == SD.testQE || testType == SD.testIAndS ||testType == SD.testBT ) {
            tests = _db.CameronTests.Where(test => test.TestType == testType)
                .ToList();
        }
        else {
            tests = _db.CameronTests.ToList();
        }
        
        return View(tests);
    }
}