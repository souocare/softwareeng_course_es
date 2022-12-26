using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence.Repositories
{
    public class CampaingsRepository : Repository<Campaings>, ICampaingsRepository
    {
        public CampaingsRepository(AdChimeContext context) : base(context)
        {

        }

        public IEnumerable<Campaings> GetAllCampaings()
        {
            return AdChimeContext.Campaings.OrderByDescending(x => x.updatedate).ToList();
        }
        


        public AdChimeContext AdChimeContext
        {
            get { return Context as AdChimeContext;  }
        }

    }
}