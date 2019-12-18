using System;
using System.Collections.Generic;

namespace AdExchangeAnalyzer.Models
{
    public class DataPoint
    {
        public DateTime Timestamp { get; set; }
        public float Value { get; set; }
    }

    public class DataRequest
    {
        public string Granularity { get; set; }
        public List<DataPoint> Series { get; set; }
        public double MaxAnomalyRatio { get; set; }
        public int Sensitivity { get; set; }
    }

    public class DataResult
    {
        public float[] ExpectedValues { get; set; }
        public bool[] IsAnomaly { get; set; }
        public bool[] IsNegativeAnomaly { get; set; }
        public bool[] IsPositiveAnomaly { get; set; }
        public float[] LowerMargins { get; set; }
        public int Period { get; set; }
        public float[] UpperMargins { get; set; }
    }

    public class DataPointEx
    {
        public DateTime Timestamp { get; set; }
        public float Value { get; set; }
        public bool IsAnomaly { get; set; }
    }
}
