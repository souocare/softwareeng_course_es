using AdChimeProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdChimeProject.Persistence.Repositories
{
    public class TemplateSMSRepository : Repository<TemplateSMS>, ITemplateSMSRepository
    {
        public TemplateSMSRepository(AdChimeContext context) : base(context)
        {

        }

        public IEnumerable<TemplateSMS> GetAllTemplates()
        {
            return AdChimeContext.TemplateSMS.Where(x => x.isaproved == true).OrderByDescending(x => x.updatedate);
        }


        public IEnumerable<TemplateSMS> GetTemplateInfo(int id)
        {
            return AdChimeContext.TemplateSMS.Where(c => c.idtemplate == id);
        }

        public AdChimeContext AdChimeContext
        {
            get { return Context as AdChimeContext;  }
        }

    }
}