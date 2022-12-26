using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdChimeProject.Core.Repositories
{
    public interface ISMSCounterRepository : IRepository<tSMSCounter>
    {
        IEnumerable<tSMSCounter> GetSMSCounter();
    }
}
