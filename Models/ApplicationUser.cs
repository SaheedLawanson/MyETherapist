using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace etherapist.Models;

public class ApplicationUser: IdentityUser {
    [Required]
    public String FirstName { get; set; }
    [Required]
    public String LastName { get; set; }
    
}