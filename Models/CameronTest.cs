using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace etherapist.Models;

public class CameronTest {
    [Key]
    public Int32 Id { get; set; } 
    
    [Required]
    public String UserId { get; set; }
    [ForeignKey("UserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }

    public String Result { get; set; }

    public String TestType { get; set; }

    public String CreatedAt { get; set; }
    public String Status { get; set; }
}