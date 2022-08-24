using etherapist.Models;

namespace etherapist.Models.ViewModels;

public class SubscriptionVM {
    public List<PatientSubscription>? PSubscriptions { get; set; }
    public PatientSubscription? ActiveSubscription { get; set; }
    public List<Subscription>? Subscriptions { get; set; }
    public PatientSubscription? PSubscription { get; set; }
}