using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace etherapist.Models;

public class CameronQuestion {
    [Key]
    public Int32 Id { get; set; } 

    [Required]
    public String OptionA { get; set; }
    [Required]
    public String OptionB { get; set; }
    [Required]
    public String OptionC { get; set; }
    [Required]
    public String OptionD { get; set; }
    [Required]
    public String OptionAValue { get; set; }
    [Required]
    public String OptionBValue { get; set; }
    [Required]
    public String OptionCValue { get; set; }
    [Required]
    public String OptionDValue { get; set; }

}