using System.Configuration;
using Ldm.Charting.Data;
using Ldm.Charting.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;


namespace Ldm.Charting.Web.Controllers
{
        public class ChartController : Controller
        {
        enum AlertImportance
        {
            Low  = 500,
            Medium = 750,
            High =  1000
        }
    
        public ActionResult Index()
        {
            return View();
        }

        public FileResult CreateVicImagesChart(int width = 1000, int height = 1000, string colour = "Black")
        {
            IVicImageCountsRepository repo = new VicImageCountsRepository();
            IQueueCounterRepository queueCounterRepo = new QueueCounterRepository();
            var VicImageCounts = repo.GetVicImageCounts();

            Chart chart = ChartBuilder.BuildChart(SeriesChartType.Line, width, height);

            // Bind Data
            foreach (var ImageCount in VicImageCounts)
            {
                chart.Series[0].Points.AddY(ImageCount.Minutes);
            }
            // Monitor board color changes based upon order update queue accordingly 
            chart.BackColor = Color.FromName(colour);
            chart.ChartAreas.ToList().ForEach(m => m.BackColor = Color.FromName(colour));

            // Save to MemoryStream and pass contents back to View.
            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }
        [AllowAnonymous]
        public JsonResult BackGroundColor()
        {
            IQueueCounterRepository repo = new QueueCounterRepository();
            var orderUpdateQueueLength = (repo.GetOrderUpdatesCountWhere(@"Identifier NOT LIKE '%WcfReceiver_OfficeExpressIntegrationService%'") > 0 ? repo.GetOrderUpdatesCountWhere(@"Identifier NOT LIKE '%WcfReceiver_OfficeExpressIntegrationService%'") : 0) ;
            var color = "Black";

            if (orderUpdateQueueLength > (int)AlertImportance.Low)
                color = "Goldenrod";

            if (orderUpdateQueueLength > (int)AlertImportance.Medium)
                color = "DarkOrange";

            if (orderUpdateQueueLength > (int)AlertImportance.High)
                color = "Red";

            var chartAlert = new ChartAlert() { ChartColour = color, OrderUpdatesRows = orderUpdateQueueLength.ToString() };
            return Json(chartAlert, JsonRequestBehavior.AllowGet); 
                
        }
        
        public FileResult CreateQueueCountsChart(int width = 1000, int height = 1000, string colour = "Black")
        {
            IQueueCounterRepository repo = new QueueCounterRepository();
            IQueueCounterRepository repoMaple = new QueueCounterRepository(ConfigurationManager.ConnectionStrings["maple"].ConnectionString);
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

            // Monitor board color changes based upon order update queue accordingly 
             chart.BackColor = Color.FromName(colour);
             chart.ChartAreas.ToList().ForEach(m => m.BackColor = Color.FromName(colour));      

            // Set 
            var maximum = currentSeries.Points.Select(x => x.YValues.First()).Max();
            chart.ChartAreas[0].AxisY.Maximum = Math.Max(maximum + 10, 150);

            // Save to MemoryStream and pass contents back to View.
            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }
        public FileResult CreatePencilQueueCountsChart(int width = 1000, int height = 1000, string colour = "Black")
        {
            IQueueCounterRepository repo = new QueueCounterRepository();
            IQueueCounterRepository repoMaple = new QueueCounterRepository(ConfigurationManager.ConnectionStrings["maple"].ConnectionString);
            Chart chart = ChartBuilder.BuildChart(SeriesChartType.Bar, width, height);

            var currentSeries = chart.Series[0];

            // Bind Data
            currentSeries.Points.AddXY("Pencil OrderToDispatch", repo.GetQueueFromQuery("select count(*) from pencil.dbo.RunQueueOrderToDispatch WITH (NOLOCK) where RequiresAttention = 0"));
            currentSeries.Points.AddXY("Pencil Forms Building", repo.GetQueueFromQuery("select count(*) from pencil.dbo.RunRequestForm WITH (NOLOCK) where Built = 0"));
            currentSeries.Points.AddXY("Pencil Automation", repoMaple.GetQueueFromQuery("select count(*) from pencil.dbo.OrderLock WITH (NOLOCK) where UserName = 'Automation'"));
            currentSeries.Points.AddXY("PENCIL Order Updates", repo.GetQueueCount("[//PENCIL/OrderUpdateRequestTargetQueue]"));
            currentSeries.Points.AddXY("Pencil Create", repo.GetQueueFromQuery("select count(*) from [//PENCIL/OrderCreateRequestTargetQueue] WITH (NOLOCK)"));
            currentSeries.Points.AddXY("Maple Order Updates", repoMaple.GetQueueCount("[//Maple/OrderUpdateRequestTargetQueue]"));

            // Monitor board color changes based upon order update queue accordingly 
            chart.BackColor = Color.FromName(colour);
            chart.ChartAreas.ToList().ForEach(m => m.BackColor = Color.FromName(colour));

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

        public JsonResult GetErrors()
        {
            ILoggingDataRepository repo = new LoggingDataRepository();
            var allErrorsOverThreshold = repo.GetAllErrorsOverThreshold(
                int.Parse(ConfigurationManager.AppSettings.Get("loggingCountThreshold")),
                int.Parse(ConfigurationManager.AppSettings.Get("loggingTimingThreshold")),
                int.Parse(ConfigurationManager.AppSettings.Get("maxScanPeriod")));
            return Json(allErrorsOverThreshold);
        }
    }

}
