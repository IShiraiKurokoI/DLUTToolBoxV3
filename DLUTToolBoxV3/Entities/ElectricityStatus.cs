using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLUTToolBoxV3.Entities
{
    public class ElectricityStatus
    {
        public string message { get; set; }
        public Resultdata resultData { get; set; }
        public bool success { get; set; }
        public class Resultdata
        {
            public string sydl { get; set; }
        }

    }
}
