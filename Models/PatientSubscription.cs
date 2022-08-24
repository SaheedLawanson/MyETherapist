using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace etherapist.Models;

public class PatientSubscription {
    [Key]
    public Int32 Id { get; set; } 
    
    [Required]
    public String PatientId { get; set; }
    [ForeignKey("PatientId")]
    public ApplicationUser Patient { get; set; }
    
    [Required]
    public Int32 SubscriptionId { get; set; }
    [ForeignKey("SubscriptionId")]
    public Subscription Subscription { get; set; }

    public Int32 SessionsLeft { get; set; }

    public String Status { get; set; }

    public String PaymentStatus { get; set; }
    public String? PaymentSessionId { get; set; }
    public String? PaymentIntentId { get; set; }
}