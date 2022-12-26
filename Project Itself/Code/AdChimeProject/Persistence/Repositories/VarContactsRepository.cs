using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence.Repositories
{
    public class VarContactsRepository : Repository<tVarContact>, IVarContactsRepository
    {
        public VarContactsRepository(AdChimeContext context) : base(context)
        {

        }

        public List<string> GetAllVariableNames()
        {
            return AdChimeContext.tVarContacts.Select(x => x.VarName).ToList();
        }

        public string GetColType_Variable(string variable)
        {
            return AdChimeContext.tVarContacts.Where(x => x.VarName == variable).Select(x => x.colTypeFilter).FirstOrDefault();
        }

        public AdChimeContext AdChimeContext
        {
            get { return Context as AdChimeContext;  }
        }

    }
}