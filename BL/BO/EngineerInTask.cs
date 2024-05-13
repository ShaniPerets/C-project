using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///<summary>
///help entity-if there is a engineer for task we want his deatils
///</summary>
namespace BO
{
    public class EngineerInTask
    {
        public int Id {  get; init; }//engineer id
        public  string Name { get; init; }//engineer name

        // Override ToString() method
        public override string ToString()
        {
            return Tools.ToStringProperty(this); // Call the ToStringProperty method from Tools
        }
    }

    
}
