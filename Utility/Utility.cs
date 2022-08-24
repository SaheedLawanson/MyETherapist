namespace etherapist.Utility;

public static class SD {
    public const String Role_Patient = "Patient";
    public const String Role_Therapist = "Therapist";
    public const String Role_Admin = "Admin";

    public const Int32 session_awaitingTherapistAssign = 1;
    public const Int32 session_awaitingTherapistApproval = 2;
    public const Int32 session_completed = 3;
    public const Int32 session_sessionCompleted = 4;
    public const Int32 session_rated = 5;
    public const Int32 session_end = 6;
    
    public const String subscription_active = "active";
    public const String subscription_inactive = "inactive";

    public const String condition1= "Depression";
    public const String condition2= "Anxiety";

    public const String testIAndS = "Intensive and Stretched";
    public const String testQE = "Quick Estimate";
    public const String testBT = "Biased Test";

    public const String test_status_processing = "processing";
    public const String test_status_ready = "ready";

    public const String subPaytStatus_success = "success";
    public const String subPaytStatus_failed = "failed";
    public const String subPaytStatus_pending = "pending";
}