using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// The class contains attributes that correspond to the general configuration data we recorded in previous steps
/// NextDependencyId get the current running id of dependencies and increase it.
/// NextTaskId get the current running id of tasks and increase it.
/// </summary>

namespace Dal
{
    internal static class Config
    {
        static string s_data_config_xml = "data-config";
        internal static DateTime? startDate
        {
            get => XMLTools.GetStartDate(s_data_config_xml, "startDate");
        }
        internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
        internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    }

}
