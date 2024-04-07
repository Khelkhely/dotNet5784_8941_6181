using System.Diagnostics.Contracts;

namespace BO;

public enum EngineerExperience
{
    Beginner,
    AdvancedBeginner,
    Intermediate,
    Advanced,
    Expert,
    None
}
public enum Status
{
    Unscheduled,
    Scheduled,
    OnTrack,
    //InJeopardy,
    Done
}