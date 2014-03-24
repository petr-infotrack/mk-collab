using Ldm.Charting.Data;
using Ldm.Charting.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;


namespace Ldm.Charting.Web.Controllers
{
    public class ChartController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public FileResult CreateVicImagesChart(int width = 1000, int height = 1000)
        {
            IVicImageCountsRepository repo = new VicImageCountsRepository();
            var VicImageCounts = repo.GetVicImageCounts();

            Chart chart = ChartBuilder.BuildChart(SeriesChartType.Line, width, height);

            // Bind Data
            foreach (var ImageCount in VicImageCounts)
            {
                chart.Series[0].Points.AddY(ImageCount.Minutes);
            }

            // Save to MemoryStream and pass contents back to View.
            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }
        public FileResult CreateQueueCountsChart(int width = 1000, int height = 1000)
        {
            IQueueCounterRepository repo = new QueueCounterRepository();

            Chart chart = ChartBuilder.BuildChart(SeriesChartType.Bar, width, height);

            var currentSeries = chart.Series[0];

            // Bind Data
            currentSeries.Points.AddXY("Order Execution Tasks", repo.GetQueueCount("[OrderExecutionTasks]"));
            currentSeries.Points.AddXY("Order Updates", repo.GetOrderUpdatesCountWhere(@"Identifier NOT LIKE '%WcfReceiver_OfficeExpressIntegrationService%'"));
            currentSeries.Points.AddXY("Order Update Responses", repo.GetQueueCount("[OrderUpdateResponseQueue]"));
            currentSeries.Points.AddXY("Order Update Requests", repo.GetQueueCount("[OrderUpdateRequestQueue]"));
            currentSeries.Points.AddXY("OfficeExpressIntegration", repo.GetOrderUpdatesCount("WcfReceiver_OfficeExpressIntegrationService"));
            currentSeries.Points.AddXY("FileTrack Logins", repo.GetQueueCount("[FileTrackLoginUpdates]"));
            currentSeries.Points.AddXY("FileTrack Orders", repo.GetQueueCount("[FileTrackOrderUpdates]"));
            currentSeries.Points.AddXY("PENCIL", repo.GetQueueCount("[//PENCIL/OrderUpdateRequestTargetQueue]"));
            currentSeries.Points.AddXY("PDMSNSW", repo.GetQueueCount("[//PDMSNSW/OrderUpdateRequestTargetQueue]"));

            // Set 
            var maximum = currentSeries.Points.Select(x => x.YValues.First()).Max();
            chart.ChartAreas[0].AxisY.Maximum = Math.Max(maximum + 10, 150);

            // Save to MemoryStream and pass contents back to View.
            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        // Test JSON output for Google Charts.
        public JsonResult GetVicImagesChartData()
        {
            IVicImageCountsRepository repo = new VicImageCountsRepository();
            var VicImageCounts = repo.GetVicImageCounts().Select(x => x.Minutes);
            return Json(VicImageCounts);
        }
    }

}
