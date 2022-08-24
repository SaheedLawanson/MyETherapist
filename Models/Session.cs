using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace etherapist.Models;

public class Session {
    [Key]
    public Int32 Id { get; set; } 
    [Required]
    public String MentalCondition { get; set; }

    public String PatientId { get; set; }
    [ForeignKey("PatientId")]
    [ValidateNever]
    public ApplicationUser Patient { get; set; }

    public String? TherapistId { get; set; }
    [ForeignKey("TherapistId")]
    [ValidateNever]
    public ApplicationUser? Therapist { get; set; }

    public String? StartTime { get; set ;}

    public String? StopTime { get; set ;}

    public DateTime ScheduledTime { get; set; }

    public TimeSpan MeetingSpan { get; set; }

    public String? Comment { get; set; }

    public String MeetingType { get; set; }

    public Int32 Status { get; set; }
    public Int32? Rating { get; set; }


    public Int32? PatientSubscriptionId { get; set; }
    [ForeignKey("PatientSubscriptionId")]
    [ValidateNever]
    public PatientSubscription? PatientSubscription { get; set; }

    public Int32 SubscriptionId { get; set; }
    [ForeignKey("SubscriptionId")]
    [ValidateNever]
    public Subscription Subscription { get; set; }
    public String? SpecialNeeds { get; set; }
}