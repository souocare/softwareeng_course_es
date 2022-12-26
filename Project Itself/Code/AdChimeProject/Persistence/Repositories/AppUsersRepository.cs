using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence.Repositories
{
    public class AppUsersRepository : Repository<AppUsers>, IAppUsersRepository
    {
        public AppUsersRepository(AdChimeContext context) : base(context)
        {

        }

        public IEnumerable<AppUsers> GetUser(string email)
        {
            return AdChimeContext.AppUsers.Where(c => c.Email == email).ToList();
        }
        

        public AdChimeContext AdChimeContext
        {
            get { return Context as AdChimeContext;  }
        }

    }
}