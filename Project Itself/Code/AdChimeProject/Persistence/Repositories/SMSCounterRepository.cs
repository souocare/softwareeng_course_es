using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence.Repositories
{
    public class SMSCounterRepository : Repository<tSMSCounter>, ISMSCounter
    {
        public SMSCounterRepository(AdChimeContext context) : base(context)
        {

        }

        public IEnumerable<tSMSCounter> GetSMSCounter()
        {
            return AdChimeContext.tSMSCounters.Where(c => c.idcounter == 1).ToList();
        }
        

        public AdChimeContext AdChimeContext
        {
            get { return Context as AdChimeContext;  }
        }

    }
}