namespace GatePassCancellation.Models
{
    public class TruckInOutDetail
    {
        public string DONumber { get; set; }
        public DateTime Date { get; set; }
        public string GatePassNumber { get; set; }
        public DateTime GatePassDate { get; set; }
        public string TruckNo { get; set; }
        public DateTime TruckInDate { get; set; }
        public DateTime TruckOutDate { get; set; }
    }
}
