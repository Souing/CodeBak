using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.Sys.DbModel
{
    public class Test:BaseEntity<Guid>
    {
        [StringLength(50)]
        public  string Name { get; set; }


        public int Age { get; set; }
    }
}
