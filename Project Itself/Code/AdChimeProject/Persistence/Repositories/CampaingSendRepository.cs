using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence.Repositories
{
    public class CampaingSendRepository : Repository<tCampaignSend>, ICampaingSendRepository
    {
        public CampaingSendRepository(AdChimeContext context) : base(context)
        {

        }

        public IEnumerable<tCampaignSend> GetAllCampaings()
        {
            return AdChimeContext.tCampaignSends.ToList();
        }
        


        public AdChimeContext AdChimeContext
        {
            get { return Context as AdChimeContext;  }
        }

    }
}