using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdChimeProject.Core.Repositories
{
    public interface ILinksRepository : IRepository<sLink>
    {

        string GetID_ShorterLink(string link);
    }
}
