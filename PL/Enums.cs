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
