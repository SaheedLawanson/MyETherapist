using etherapist.Models;
using etherapist.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace etherapist.Models.ViewModels;

public class SessionVM {
    public Session? session { get; set; }
    public List<Session>? sessions { get; set; }
    public IEnumerable<SelectListItem>? mentalConditionList = new List<String>() {
        SD.condition1, SD.condition2
        }.Select(cond => new SelectListItem{Text=cond, Value=cond});
    

    public String? Date { get; set; }
    public String? Time { get; set; }
    public List<SelectListItem> therapists { get; set; }
}