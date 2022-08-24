using etherapist.Models;
using etherapist.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace etherapist.Models.ViewModels;

public class CameronVM {
    public List<CameronQuestion> questions { get; set; }
    public List<CameronTest> tests { get; set; }

    public String question_1 { get; set; }
    public String question_2 {get; set; }
}