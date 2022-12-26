using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdChimeProject.Core.Repositories
{
    public interface ITemplateSMSRepository : IRepository<TemplateSMS>
    {
        IEnumerable<TemplateSMS> GetTemplateInfo(int id);
        IEnumerable<TemplateSMS> GetAllTemplates();
    }
}
