using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///<summary>
///help entity-if there is a task for engineer we want the task deatuls
///</summary>
namespace BO
{
    public class TaskInEngineer
    {
        public int Id { get; init; }//task id
        public  string Alias { get; set; }//task alias

        // Override ToString() method
        public override string ToString()
        {
            return Tools.ToStringProperty(this); // Call the ToStringProperty method from Tools
        }
    }


}
