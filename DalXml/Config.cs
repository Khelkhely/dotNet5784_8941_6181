namespace Dal;

static internal class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId 
    { 
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId");
        set => XMLTools.SetId(s_data_config_xml, "NextTaskId", value);
    }
    internal static int NextDependencyId 
    { 
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId");
        set => XMLTools.SetId(s_data_config_xml, "NextDependencyId", value);
    }
    internal static DateTime? projectStartDate
    {
        get => XMLTools.GetScheduleDate(s_data_config_xml, "StartDate");
        set => XMLTools.SetScheduleDate(s_data_config_xml, "StartDate", value);
    }
    internal static DateTime? projectEndDate
    {
        get => XMLTools.GetScheduleDate(s_data_config_xml, "EndDate");
        set => XMLTools.SetScheduleDate(s_data_config_xml, "EndDate", value);
    }

}
