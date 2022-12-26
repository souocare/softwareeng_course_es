using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence.Repositories
{
    public class RecipientsListsRepository : Repository<RecipientsLists>, IRecipientsListsRepository
    {
        public RecipientsListsRepository(AdChimeContext context) : base(context)
        {

        }

        public IEnumerable<RecipientsLists> GetRecipientsLists()
        {
            return AdChimeContext.RecipientsLists.ToList();
        }
        


        public AdChimeContext AdChimeContext
        {
            get { return Context as AdChimeContext;  }
        }

    }
}