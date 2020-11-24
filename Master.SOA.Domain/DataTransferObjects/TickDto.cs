using System;

namespace Master.SOA.Domain.DataTransferObjects
{
    public class TickDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public InstrumentDto Instrument { get; set; }
    }
}