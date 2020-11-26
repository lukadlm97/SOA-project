using System;

namespace Master.SOA.Domain.DataTransferObjects
{
    public class TickDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public double OpenPrice { get; set; }
        public double ClosePrice { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public InstrumentDto Instrument { get; set; }
    }
}