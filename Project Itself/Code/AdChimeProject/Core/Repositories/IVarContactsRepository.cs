using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdChimeProject.Core.Repositories
{
    public interface IVarContactsRepository : IRepository<tVarContact>
    {
        List<string> GetAllVariableNames();
        string GetColType_Variable(string variable);
    }
}
