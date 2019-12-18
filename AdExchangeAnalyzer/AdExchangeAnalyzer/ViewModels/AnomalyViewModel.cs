using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using AdExchangeAnalyzer.Models;
using AdExchangeAnalyzer.Services;

using SkiaSharp;
using Microcharts;
using Xamarin.Forms;

namespace AdExchangeAnalyzer.ViewModels
{
    public class AnomalyViewModel : BaseViewModel
    {
        public Command AnalyzeDataCommand { get; set; }

        private int sensitivity;

        public int Sensitivity
        {
            get { return sensitivity; }
            set { sensitivity = value; OnPropertyChanged(); }
        }

        private DataRequest dataRequest;

        public DataRequest DataRequest
        {
            get { return dataRequest; }
            set { dataRequest = value; OnPropertyChanged(); }
        }

        private DataResult dataResult;

        public DataResult DataResult
        {
            get { return dataResult; }
            set { dataResult = value; OnPropertyChanged(); }
        }

        private ObservableCollection<DataPoint> dataPoints;

        public ObservableCollection<DataPoint> DataPoints
        {
            get { return dataPoints; }
            set { dataPoints = value; OnPropertyChanged(); }
        }

        private ObservableCollection<DataPointEx> dataPointEx;

        public ObservableCollection<DataPointEx> DataPointEx
        {
            get { return dataPointEx; }
            set { dataPointEx = value; OnPropertyChanged(); }
        }

        private Chart chart;

        public Chart Chart
        {
            get { return chart; }
            set { chart = value; OnPropertyChanged(); }
        }

        public AnomalyViewModel()
        {
            IsBusy = true;
            GetData();
            CreateChart(anomalies: false);
            AnalyzeDataCommand = new Command(async () => await AnalyzeData());
            IsBusy = false;
        }

        private void GetData()
        {
            var series = FileService.ReadFile();
            DataPoints = new ObservableCollection<DataPoint>(series);

            Sensitivity = 95;

            DataRequest = new DataRequest()
            {
                Granularity = "hourly",
                MaxAnomalyRatio = 0.25,
                Sensitivity = Sensitivity,
                Series = series
            };
        }

        private void CreateChart(bool anomalies)
        {
            Chart = null;

            Chart = new LineChart()
            {
                LineMode = LineMode.Spline,
                LabelTextSize = 0
            };

            Chart.Entries = DataRequest.Series.Select(
                (v, index) => new Microcharts.Entry(v.Value)
                {
                    Label = v.Timestamp.ToString("MM/dd_HH:mm"),
                    Color = anomalies
                                ? DataPointEx.Any(x => x.Timestamp.ToString("MM/dd_HH:mm") == v.Timestamp.ToString("MM/dd_HH:mm"))
                                    ? SKColors.Red 
                                    : SKColors.Green
                                : SKColors.Green, 
                });
        }

        private async Task AnalyzeData()
        {
            IsBusy = true;
            DataPointEx = new ObservableCollection<DataPointEx>();

            DataRequest.Sensitivity = Sensitivity;
            DataResult = await AnomalyDetectorService.AnalyzeData(DataRequest);

            if (DataResult != null)
            {
                for (int i = 0; i < DataResult.IsAnomaly.Length; i++)
                {
                    if (DataResult.IsAnomaly[i])
                    {
                        var point = DataRequest.Series[i];

                        DataPointEx.Add(new DataPointEx()
                        {
                            Value = point.Value,
                            Timestamp = point.Timestamp,
                            IsAnomaly = true
                        });
                    }
                }

                CreateChart(anomalies: true);
            }

            IsBusy = false;
        }
    }
}
