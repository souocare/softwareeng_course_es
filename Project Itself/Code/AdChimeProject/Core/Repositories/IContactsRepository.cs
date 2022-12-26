using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdChimeProject.Core.Repositories
{
    public interface IContactsRepository : IRepository<Contacts>
    {
        IEnumerable<Contacts> GetContactsWithOptin();
        IList<Contacts> GetInfoContact(int id);
        Contacts GetInfoContactEdit(int id);

    }
}
