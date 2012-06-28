namespace Kendo.Mvc.UI
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.UI;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.Infrastructure;
    using Kendo.Mvc.UI.Html;

    public class Chart<T> : ViewComponentBase, IChart where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Chart{T}" /> class.
        /// </summary>
        /// <param name="viewContext">The view context.</param>
        /// <param name="IJavaScriptInitializer">The javascript initializer.</param>
        /// <param name="urlGenerator">The URL Generator.</param>
        public Chart(ViewContext viewContext, IJavaScriptInitializer initializer, IUrlGenerator urlGenerator)
            : base(viewContext, initializer)
        {
            UrlGenerator = urlGenerator;
            Title = new ChartTitle();
            ChartArea = new ChartArea();
            PlotArea = new PlotArea();
            Legend = new ChartLegend();
            Series = new List<ChartSeriesBase<T>>();
            CategoryAxis = new ChartCategoryAxis<T>(this);
            ValueAxes = new List<IChartValueAxis>();
            XAxes = new List<IChartValueAxis>();
            YAxes = new List<IChartValueAxis>();
            SeriesDefaults = new ChartSeriesDefaults<T>(this);
            AxisDefaults = new ChartAxisDefaults<T>(this);
            Tooltip = new ChartTooltip();
            Transitions = true;
            DataSource = new DataSource();
            DataSource.Schema.Data = "";
            DataSource.Schema.Total = "";
            DataSource.Schema.Errors = "";
            DataSource.ModelType(typeof(T));
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public IEnumerable<T> Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL generator.
        /// </summary>
        /// <value>The URL generator.</value>
        public IUrlGenerator UrlGenerator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the Chart area.
        /// </summary>
        /// <value>
        /// The Chart area.
        /// </value>
        public ChartArea ChartArea
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the Plot area.
        /// </summary>
        /// <value>
        /// The Plot area.
        /// </value>
        public PlotArea PlotArea
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the Chart theme.
        /// </summary>
        /// <value>
        /// The Chart theme.
        /// </value>
        public string Theme
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Chart title.
        /// </summary>
        /// <value>
        /// The Chart title.
        /// </value>
        public ChartTitle Title
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the Chart legend.
        /// </summary>
        /// <value>
        /// The Chart legend.
        /// </value>
        public ChartLegend Legend
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the Chart transitions.
        /// </summary>
        /// <value>
        /// The Chart Transitions.
        /// </value>
        public bool Transitions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the chart series.
        /// </summary>
        public IList<ChartSeriesBase<T>> Series
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default settings for all series.
        /// </summary>
        public ChartSeriesDefaults<T> SeriesDefaults
        {
            get;
            private set;
        }

        /// <summary>
        /// Configuration for the default category axis (if any)
        /// </summary>
        public IChartCategoryAxis CategoryAxis
        {
            get;
            set;
        }

        /// <summary>
        /// Configuration for all value axes
        /// </summary>
        public IList<IChartValueAxis> ValueAxes
        {
            get;
            set;
        }

        /// <summary>
        /// Configuration for all X axes in scatter charts
        /// </summary>
        public IList<IChartValueAxis> XAxes
        {
            get;
            set;
        }

        /// <summary>
        /// Configuration for all Y axes in scatter charts
        /// </summary>
        public IList<IChartValueAxis> YAxes
        {
            get;
            set;
        }

        /// <summary>
        /// Configuration for the default axis 
        /// </summary>
        public IChartAxisDefaults AxisDefaults
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets the data source configuration.
        /// </summary>
        public DataSource DataSource
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the series colors.
        /// </summary>
        public IEnumerable<string> SeriesColors
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data point tooltip options
        /// </summary>
        public ChartTooltip Tooltip
        {
            get;
            set;
        }

        /// <summary>
        /// Writes the initialization script.
        /// </summary>
        /// <param name="writer">The writer object.</param>
        public override void WriteInitializationScript(TextWriter writer)
        {
            var options = new Dictionary<string, object>(Events);

            SerializeData("chartArea", ChartArea.CreateSerializer().Serialize(), options);
            SerializeData("plotArea", PlotArea.CreateSerializer().Serialize(), options);

            SerializeTheme(options);

            SerializeData("title", Title.CreateSerializer().Serialize(), options);

            SerializeData("legend", Legend.CreateSerializer().Serialize(), options);

            SerializeSeries(options);

            SerializeData("seriesDefaults", SeriesDefaults.CreateSerializer().Serialize(), options);

            SerializeData("axisDefaults", AxisDefaults.CreateSerializer().Serialize(), options);

            SerializeData("categoryAxis", CategoryAxis.CreateSerializer().Serialize(), options);

            SerializeAxes("valueAxis", ValueAxes, options);

            SerializeAxes("xAxis", XAxes, options);

            SerializeAxes("yAxis", YAxes, options);

            SerializeTransitions(options);

            SerializeDataSource(options);

            SerializeSeriesColors(options);

            SerializeData("tooltip", Tooltip.CreateSerializer().Serialize(), options);

            writer.Write(Initializer.Initialize(Selector, "Chart", options));

            base.WriteInitializationScript(writer);
        }

        private void SerializeData(string key, IDictionary<string, object> data, IDictionary<string, object> options)
        {
            if (data.Count > 0)
            {
                options.Add(key, data);
            }
        }

        private void SerializeTheme(IDictionary<string, object> options)
        {
            if (Theme.HasValue())
            {
                options.Add("theme", Theme);
            }
        }

        private void SerializeSeries(IDictionary<string, object> options)
        {
            if (Series.Count > 0)
            {
                var serializedSeries = new List<IDictionary<string, object>>();
                foreach (var s in Series)
                {
                    serializedSeries.Add(s.CreateSerializer().Serialize());
                }

                options.Add("series", serializedSeries);
            }
        }

        private void SerializeDataSource(IDictionary<string, object> options)
        {
            if (!string.IsNullOrEmpty(DataSource.Transport.Read.Url))
            {
                if (!DataSource.Transport.Read.Type.HasValue())
                {
                    DataSource.Transport.Read.Type = "POST";
                }                

                options.Add("dataSource", DataSource.ToJson());
            }
            else if (Data != null)
            {
                options.Add("dataSource", new Dictionary<string, object> { { "data", Data } });
            }
        }

        private void SerializeSeriesColors(IDictionary<string, object> options)
        {
            if (SeriesColors != null)
            {
                options.Add("seriesColors", SeriesColors);
            }
        }

        private void SerializeTransitions(IDictionary<string, object> options)
        {
            if (!Transitions)
            {
                options.Add("transitions", Transitions);
            }
        }

        private void SerializeAxes<TAxis>(string key, IList<TAxis> axes, IDictionary<string, object> options)
            where TAxis : IChartAxisBase
        {
            if (axes.Count > 0)
            {
                var serializedAxes = new List<IDictionary<string, object>>();
                var shouldSerialize = false;

                for (var i = 0; i < axes.Count; i++)
                {
                    var a = axes[i];
                    var data = a.CreateSerializer().Serialize();
                    var isPrimary = i == 0 && axes.Count > 1;

                    if (data.Count > 0 || isPrimary)
                    {
                        serializedAxes.Add(data);
                        shouldSerialize = true;
                    }
                }

                if (shouldSerialize)
                {
                    options.Add(key, serializedAxes);
                }
            }
        }

        /// <summary>
        /// Writes the Chart HTML.
        /// </summary>
        /// <param name="writer">The writer object.</param>
        protected override void WriteHtml(HtmlTextWriter writer)
        {
            if (!HtmlAttributes.ContainsKey("id"))
            {
                HtmlAttributes["id"] = Id;
            }

            new ChartHtmlBuilder<T>(this)
                .Build()
                .WriteTo(writer);

            base.WriteHtml(writer);
        }
    }
}