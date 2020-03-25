using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlertsAdmin.Controllers
{
    public class ConstantsController : Controller
    {
        private readonly IConfiguration _config;

        public ConstantsController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var appSettings = _config.GetSection("AppSettings").GetChildren().ToDictionary(x => x.Key, x => x.Value);
            var json = JsonConvert.SerializeObject(appSettings);
            return new JavaScriptResult($"var constants = {json};");
        }
    }
    public class JavaScriptResult : ContentResult
    {
        public JavaScriptResult(string script)
        {
            this.Content = script;
            this.ContentType = "application/javascript";
        }
    }
}
