using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Windows.Forms;

namespace IoTClient.Tool
{
    public partial class BrokenLineChart : Form
    {
        public BrokenLineChart(string title)
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            //FormBorderStyle = FormBorderStyle.FixedSingle;

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)
                .Y(model => model.Value);

            Charting.For<MeasureModel>(mapper);

            ChartValues = new ChartValues<MeasureModel>();
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = ChartValues,
                    PointGeometrySize = 1,
                    StrokeThickness = 2,
                    Title ="地址: "+ title+"  值:"
                }
            };
            cartesianChart1.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => new DateTime((long)value).ToString("mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(5).Ticks
                }
            });

            SetAxisLimits(DateTime.Now);
        }

        public ChartValues<MeasureModel> ChartValues { get; set; }

        private void SetAxisLimits(DateTime now)
        {
            var step = ChartValues.Count * 800 / 1000 / 15f;
            var new_step = step <= 1 ? 1 : step;
            cartesianChart1.AxisX[0].Separator = new Separator
            {
                Step = TimeSpan.FromSeconds(new_step).Ticks
            };
            //cartesianChart1.AxisX[0].MaxValue = now.Ticks + TimeSpan.FromSeconds(0).Ticks;
            //cartesianChart1.AxisX[0].MinValue = now.Ticks - TimeSpan.FromSeconds(60).Ticks;
        }

        public void AddData(double value)
        {

            var now = DateTime.Now;

            ChartValues.Add(new MeasureModel
            {
                DateTime = now,
                Value = Math.Round(value, 4)
            });

            SetAxisLimits(now);

            if (ChartValues.Count > 400) ChartValues.RemoveAt(0);
        }
    }
}
