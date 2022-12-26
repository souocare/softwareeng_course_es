using AdChimeProject.Core;
using AdChimeProject.Core.Repositories;
using AdChimeProject.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AdChimeContext _context;

        public UnitOfWork(AdChimeContext context)
        {
            _context = context;
            Campaings = new CampaingsRepository(_context);
            CampaingSend = new CampaingSendRepository(_context);
            Contacts = new ContactsRepository(_context);
            AppUsers = new AppUsersRepository(_context);
            VarContacts = new VarContactsRepository(_context);
            ContactsVariables = new ContactsVariablesRepository(_context);
            TemplateSMS = new TemplateSMSRepository(_context);
            RecipientsLists = new RecipientsListsRepository(_context);
            SMSCounter = new SMSCounterRepository(_context);
            Links = new LinksRepository(_context);
        }

        public ICampaingsRepository Campaings { get; protected set; }
        public ICampaingSendRepository CampaingSend { get; protected set; }
        public IContactsRepository Contacts { get; protected set; }
        public IAppUsersRepository AppUsers { get; private set; }
        public IVarContactsRepository VarContacts { get; protected set; }
        public IContactsVariablesRepository ContactsVariables { get; private set; }
        public ITemplateSMSRepository TemplateSMS { get; private set; }
        public IRecipientsListsRepository RecipientsLists { get; private set; }
        public ISMSCounterRepository SMSCounter { get; private set; }
        public ILinksRepository Links { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}