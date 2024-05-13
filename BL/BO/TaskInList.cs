using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///<summary>
///help entity-for list of dependcy list in task
///</summary>
namespace BO
{
    public class TaskInList
    {
        public int Id {  get; init; }//task id
        public string Description { get; set; }//task description
        public string Alias {  get; set; }//task alias
        public  Status StatusTaskInList { get; set; }//task status

        // Override ToString() method
        public override string ToString()
        {
            return Tools.ToStringProperty(this); // Call the ToStringProperty method from Tools
        }
    }
}
