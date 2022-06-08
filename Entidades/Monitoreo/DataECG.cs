using System;
using System.Collections.Generic;
using System.Text;

namespace API.Entity.Monitoreo
{
    public class DataECG
    {
        public List<int> dataECG { get; set; }
        public int order { get; set; }
        public string result { get; set; }
        public string labelResult { get; set; }
    }
}
