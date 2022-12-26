using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence.Repositories
{
    public class ContactsVariablesRepository : Repository<panelContactsVariable>, IContactsVariablesRepository
    {
        public ContactsVariablesRepository(AdChimeContext context) : base(context)
        {

        }

        public IEnumerable<panelContactsVariable> GetAllVariablesOfCertainContact(int id)
        {
            return AdChimeContext.panelContactsVariables.Where(c => c.idContact == id).ToList();
        }

        public List<string> GetValues_Variable(string variable)
        {
            // var listadadospaneluser = dbadchime.panelContactsVariables.Where(x => x.panelContact.optinSMS == 1).Where(x => x.tVarContact.VarName == elementvalue).Select(x => x.sValue).Distinct().ToList();
            return AdChimeContext.panelContactsVariables.Where(x => x.panelContact.optinSMS == 1).Where(x => x.tVarContact.VarName == variable).Select(x => x.sValue).Distinct().ToList();
        }


        public AdChimeContext AdChimeContext
        {
            get { return Context as AdChimeContext;  }
        }

    }
}