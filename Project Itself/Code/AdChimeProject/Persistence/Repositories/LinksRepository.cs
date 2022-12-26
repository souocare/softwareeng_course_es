using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence.Repositories
{
    public class LinksRepository : Repository<sLink>, ILinksRepository
    {
        public LinksRepository(AdChimeContext context) : base(context)
        {

        }


        public string GetID_ShorterLink(string link)
        {
            return AdChimeContext.sLinks.Where(x => x.sShorterLink == link).Select(x => x.idlink).FirstOrDefault().ToString();
        }


        public AdChimeContext AdChimeContext
        {
            get { return Context as AdChimeContext;  }
        }

    }
}