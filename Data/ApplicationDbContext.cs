using etherapist.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace etherapist.Data;

public class ApplicationDbContext: IdentityDbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    public DbSet<Session> Sessions { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<PatientSubscription> PatientSubscriptions { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<CameronQuestion> CameronQuestions { get; set; }
    public DbSet<CameronTest> CameronTests { get; set; }
}