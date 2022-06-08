using System;
using System.Collections.Generic;
using System.Text;

namespace API.Entity.Monitoreo
{
    public class RecordECG
    {
        public string id { get; set; }
        public string userID { get; set; }
        public string readDate { get; set; }
        public List<DataECG> data { get; set; }
        public int countNSR { get; set; }
        public int countARR { get; set; }
        public int countCHF { get; set; }
        public int countWindow { get; set; }
        public string type { get; set; }
        public string commentUser { get; set; }

    }
}
