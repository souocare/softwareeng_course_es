using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence.Repositories
{
    public class ContactsRepository : Repository<Contacts>, IContactsRepository
    {
        public ContactsRepository(AdChimeContext context) : base(context)
        {

        }

        public IEnumerable<Contacts> GetContactsWithOptin()
        {
            return AdChimeContext.Contacts.Where(c => c.optinSMS == 1).ToList();
        }

        public IList<Contacts> GetInfoContact(int id)
        {
            return AdChimeContext.Contacts.Where(c => c.idContact == id).ToList();
        }

        public Contacts GetInfoContactEdit(int id)
        {
            return AdChimeContext.Contacts.Where(c => c.idContact == id).FirstOrDefault();
        }


        public AdChimeContext AdChimeContext
        {
            get { return Context as AdChimeContext;  }
        }

    }
}