namespace API.Smart_Heart.Models.Monitoreo.RecordECG
{
    public class DataECGViewModel
    {
        public List<double> dataECG { get; set; }
        public int order { get; set; }
        public string result { get; set; }
        public string labelResult { get; set; }

    }
}