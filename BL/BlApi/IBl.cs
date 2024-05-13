using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///<summary>
///A main logical interface  that centers all logical layer interfaces
///</summary>

namespace BlApi
{
    public interface IBl
    {
        public DateTime? GetStartDate();
        public void SetStartDate(DateTime? sd);
        public StatusProject GetStatusProject();
        public IEngineer Engineer { get; }//engineer interface
        public ITask Task { get; }//task interface
        public void InitializeDB();  //initialzie the database
        public void ResetDB(); //reset the data base

        #region Clock
        public DateTime Clock { get;}
        void AddHourClock();
        void AddDayClock();
        void InitClock();
        #endregion
    }

}
