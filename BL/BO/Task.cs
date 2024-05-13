using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///<summary>
///main entity-task
///</summary>
namespace BO
{
    public class Task
    {
        public int Id { get; init; }//task id
        public string Name { get; set; }//task name
        public int? EngineerId { get; set; }// task's engineer id
        public EngineerExperience difficulty { get; set; }//task's engineer level
        public Status ? StatusTask { get; set; }//task status
        public List<BO.TaskInList?>? Dependencies { get; set; }//list od task's dependency
        public string TaskDescription { get; set; }//task description
        public string? Product { get; set; } = null;//product
        public string? Comments { get; set; } = null;//task's comments
        public DateTime? CreateTime { get; set; } = null;//create task time
        public DateTime? BeginWorkDateP { get; set; } = null;//Scheduled start time
        public DateTime?  BeginWorkDate { get; set; } = null;// Actual start time
        public int? WorkDuring { get; set; } = null;//work during
        public  DateTime ? DeadLine { get; set; } = null;//task dead-line
        public  DateTime? EndWorkTime { get; set; } = null;//end working time
        public  EngineerInTask? Engineer { get; set; }//get the task engineer's deatils

        // Override ToString() method
        public override string ToString()
        {
            return Tools.ToStringProperty(this); // Call the ToStringProperty method from Tools
        }

        public Task() { }

        public Task(Task t)
        {
            Id = t.Id;
            Name = t.Name;
            EngineerId = t.EngineerId;
            difficulty = t.difficulty;
            StatusTask = t.StatusTask;
            Dependencies = t.Dependencies;
            TaskDescription = t.TaskDescription;
            Product = t.Product;
            Comments = t.Comments;
            CreateTime = t.CreateTime;
            BeginWorkDate = t.BeginWorkDate;
            BeginWorkDateP= t.BeginWorkDateP;
            WorkDuring = t.WorkDuring;
            DeadLine = t.DeadLine;
            EndWorkTime = t.EndWorkTime;
            Engineer = t.Engineer;

        }
    }
}
