using BO;
using System.Collections;

namespace PL;

public class EngineerExperienceLevel : IEnumerable<BO.EngineerExperience>
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums =
        (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator<EngineerExperience> GetEnumerator() => s_enums.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => s_enums.GetEnumerator();
}

public class TaskStatus : IEnumerable<BO.Status>
{
    static readonly IEnumerable<BO.Status> s_enums =
        (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

    public IEnumerator<Status> GetEnumerator() => s_enums.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => s_enums.GetEnumerator();
}
