using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdChimeProject.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICampaingsRepository Campaings { get; }
        ICampaingSendRepository CampaingSend { get; }
        IContactsRepository Contacts { get; }
        IAppUsersRepository AppUsers { get; }
        IVarContactsRepository VarContacts { get; }
        IContactsVariablesRepository ContactsVariables { get; }
        ITemplateSMSRepository TemplateSMS { get; }
        IRecipientsListsRepository RecipientsLists { get; }
        ISMSCounterRepository SMSCounter { get; }
        ILinksRepository Links { get; }
        int Complete();
    }
}
