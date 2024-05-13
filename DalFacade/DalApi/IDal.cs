using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    /// <summary>
    /// interface wrapping all interfaces in dal. it reperesent the dal level.
    /// </summary>
    public interface IDal
    {
        IDependency Dependency { get; }
        IEngineer Engineer { get; }
        ITask Task { get; }
        public void SetStartDate(DateTime? sd);
        public DateTime? GetStartDate();
    }
}
