using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace etherapist.Models;

public class Subscription {
    [Key]
    public Int32 Id { get; set; } 
    
    [Required]
    public String PlanName { get; set; }
    
    [Required]
    public Int32 Price { get; set; }

    [Required]
    public String Benefits { get; set; }

    [Required]
    public Int32 AvailableSessions { get; set; }
}