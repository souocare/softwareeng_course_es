using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdChimeProject.Core.Repositories
{
    public interface IContactsVariablesRepository : IRepository<panelContactsVariable>
    {

        IEnumerable<panelContactsVariable> GetAllVariablesOfCertainContact(int id);


        List<string> GetValues_Variable(string variable);
    }
}
