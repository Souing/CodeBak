using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Sys.DbModel;

namespace Work.Sys.DbInterfaceService
{
    public interface ITestService:IBaseService<Guid,Test>
    {
    }
}
