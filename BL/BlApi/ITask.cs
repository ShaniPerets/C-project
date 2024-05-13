using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// a  interface of task with those action:
/// create task, read an task by a rule, read all tasks, update an task and delete task by its id.
/// </summary>

namespace BlApi
{
    public interface ITask
    {
        public int Create(BO.Task item);//create new task
        public BO.Task? Read(int id);//read task by id
        public BO.Task? ReadByFilter(Func<DO.Task, bool> filter);//read task by filter
        public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);//read all tasks with or without a filter
        public void Update(BO.Task item);//update task deatils
        public void Delete(int id);//delete task by id
        public void addDependency(int target, int dependOnTask);//add dependency to task
        public void UpdateBeginDate(int id, DateTime? bDateTask);  //update the beginning date of task
        public void Clear();//initialize
    }
}
