namespace DO;
/// <summary>
/// Information about the engineer
/// </summary>
/// <param name="ID">Unique ID number of the engineer</param>
/// <param name="Name">Engineer's full name</param>
/// <param name="Email">The email of the engineer</param>
/// <param name="Level">Engineer level</param>
/// <param name="Cost">cost per hour</param>
public record Engineer
(
    int Id,
    string? Name=null,
    string? Email=null,
    DO.EngineerExperience Level = EngineerExperience.Beginner,
    double? Cost = null
)
{
    public Engineer() : this(0) { }//empty ctor
}