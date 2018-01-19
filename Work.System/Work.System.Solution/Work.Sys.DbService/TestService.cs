using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Sys.DbInterfaceService;
using Work.Sys.DbModel;

namespace Work.Sys.DbService
{
    public class TestService:BaseService<Guid,Test> ,ITestService
    {
    }
}
