using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// main entity-engineer
/// </summary>
namespace BO
{

    public  class Engineer
    {
        public int Id { get; init; }//engineer id
        public string Name { get; set; }//engineer name
        public string EMail { get; set; }// engineer email
        public EngineerExperience Level { get; set; }//engineer level
        public double Cost { get; set; }//engineer cost per hour
        public TaskInEngineer? Task {  get; set; }//gets the engineer's task deatils

        // Override ToString() method
        public override string ToString()
        {
            return Tools.ToStringProperty(this); // Call the ToStringProperty method from Tools
        }

    }
}
