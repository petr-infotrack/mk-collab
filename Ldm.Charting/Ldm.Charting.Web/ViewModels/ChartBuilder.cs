using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;

namespace Ldm.Charting.Web.ViewModels
{
    public static class ChartBuilder
    {
        public static Chart BuildChart(SeriesChartType ChartType, int width, int height)
        {
            Chart chart = new Chart();

            chart.Width = width;
            chart.Height = height;
            chart.RenderType = RenderType.BinaryStreaming;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.BackColor = Color.Black;


            ChartArea currentChartArea = new ChartArea();

            currentChartArea.BackColor = Color.Black;

            foreach (var x in currentChartArea.Axes)
            {
                x.IsLabelAutoFit = false;
                x.LineColor = Color.White;
                x.LineWidth = 2;
                x.MajorGrid.LineColor = Color.FromArgb(100, 100, 100);
                x.MinorGrid.Enabled = true;
                x.MinorGrid.LineColor = Color.FromArgb(40, 40, 40);
                x.LabelStyle.ForeColor = Color.White;
                x.LabelStyle.Font = new Font(new FontFamily("Segoe UI"), 12.0f, FontStyle.Bold);
            }
            Series currentSeries = new Series();
            currentSeries.ChartType = ChartType;

            //Title currentTitle = new Title();
            //currentTitle.Text = titleText;
            //currentTitle.Font = new Font(new FontFamily("Segoe UI"), 24.0f, FontStyle.Bold);
            //currentTitle.ForeColor = Color.White;
            
            //chart.Titles.Add(currentTitle);
            chart.ChartAreas.Add(currentChartArea);
            chart.Series.Add(currentSeries);

            return chart;
        }
    }
}