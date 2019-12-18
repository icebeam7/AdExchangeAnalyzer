using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using AdExchangeAnalyzer.Models;

namespace AdExchangeAnalyzer.Services
{
    public static class FileService
    {
        public static List<DataPoint> ReadFile()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(FileService)).Assembly;
            var fullName = $"AdExchangeAnalyzer.Data.data.csv";
            var stream = assembly.GetManifestResourceStream(fullName);

            var series = new List<DataPoint>();

            using (var reader = new StreamReader(stream))
            {
                var line = string.Empty;

                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split(',');

                    if (data.Length == 2)
                    {
                        series.Add(new DataPoint()
                        {
                            Timestamp = DateTime.Parse(data[0]).ToUniversalTime(),
                            Value = float.Parse(data[1])
                        });
                    }
                }
            }

            return series;
        }
    }
}
