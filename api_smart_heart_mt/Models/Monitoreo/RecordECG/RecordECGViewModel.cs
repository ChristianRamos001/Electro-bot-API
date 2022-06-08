using API.Entity.Monitoreo;

namespace API.Smart_Heart.Models.Monitoreo.RecordECG
{
    public class RecordECGViewModel
    {
        public string id { get; set; }
        public string userID { get; set; }
        public DateTime readDate { get; set; }
        public List<DataECGViewModel> data { get; set; }
    }

    public class RecordECGViewModelApp
    {
        public string id { get; set; }
        public string userID { get; set; }
        public DateTime readDate { get; set; }
        public List<DataECGViewModel> data { get; set; }
        //public double percentResult { get; set; }
        public string labelResult { get; set; }
        public string subLabel { get; set; }
        public string type { get; set; }
        public string? commentUser { get; set; }
    }

    public class RecordOneECGViewModelApp
    {
        public string id { get; set; }
        public string userID { get; set; }
        public DateTime readDate { get; set; }
        public List<double> data { get; set; }
        //public double percentResult { get; set; }
        public string labelResult { get; set; }
        public string subLabel { get; set; }
        public string type { get; set; }
        public string? commentUser { get; set; }
    }

    public class RecordECGFilterRawModel
    {
        public string id { get; set; }
        public string userID { get; set; }
        public string readDate { get; set; }
        public string labelResult { get; set; }
        public string subLabel { get; set; }
        public string type { get; set; }
        public string? commentUser { get; set; }
    }

    public class DateResultModel
    {
        public string dateResult { get; set; }
        public bool isAbnormal { get; set; }

    }

    public class DateFilterModel
    {
        public DateTime datefilter{ get; set; }

    }
}
