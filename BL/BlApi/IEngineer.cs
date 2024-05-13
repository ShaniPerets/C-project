using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// a  interface of engineer with those action:
/// create engineer, read an engineer by a rule, read all engineers, update an engineer and delete engineer by its id.
/// </summary>

namespace BlApi
{
    public interface IEngineer
    {
        public int Create(BO.Engineer item);//create new engineer
        public BO.Engineer? Read(int id);//read engineer by id
        public BO.Engineer? ReadByFilter(Func<DO.Engineer, bool> filter);//read engineer by filter
        public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);//read all engineers with or without a filter
        public void Update(BO.Engineer item);//update engineer deatils
        public void Delete(int id);//delete engineer by id
        public BO.TaskInEngineer GetTheEngineerTasks(int EngineerId);//get the engineer tasks
        public void Clear();//initialize
    }
}
