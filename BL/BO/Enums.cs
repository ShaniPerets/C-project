using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///<summary>
///necessary enumerations in the definition of the logical entities.
///</summary>

namespace BO
{
    public enum EngineerExperience { Novice, AdvancedBeginner, Competent, Proficient, Expert, All };//create the EngineerExperience implement of task
    public enum Status { Unscheduled, Scheduled, OnTrack, InJeopardy ,Done};//task status
    public enum StatusProject { Planning, Middle,  Execution } //status of project
}
