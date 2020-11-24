using System;

namespace Master.SOA.Domain.Models
{
    public class Tick
    {
        public DateTime Time { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public Instrument Instrument { get; set; }
    }
}