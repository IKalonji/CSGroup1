using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherApplication.Models;

namespace WeatherApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult WeatherDetail(string City)
        {

            //Assign API KEY which received from OPENWEATHERMAP.ORG  
            string appId = "8113fcc5a7494b0518bd91ef3acc074f";

            //API path with CITY parameter and other parameters.  
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", City, appId);

            using (System.Net.WebClient client = new WebClient())
            {
                string json = client.DownloadString(url); 
        
                RootObject weatherInfo = JsonConvert.DeserializeObject<RootObject>(json);
 
                WeatherViewModel WeatherObj = new WeatherViewModel();

                WeatherObj.Country = weatherInfo.sys.country;
                WeatherObj.City = weatherInfo.name;
                WeatherObj.Lat = Convert.ToString(weatherInfo.coord.lat);
                WeatherObj.Lon = Convert.ToString(weatherInfo.coord.lon);
                WeatherObj.Description = weatherInfo.weather[0].description;
                WeatherObj.Humidity = Convert.ToString(weatherInfo.main.humidity);
                WeatherObj.Temp = Convert.ToString(weatherInfo.main.temp);
                WeatherObj.TempFeelsLike = Convert.ToString(weatherInfo.main.feels_like);
                WeatherObj.TempMax = Convert.ToString(weatherInfo.main.temp_max);
                WeatherObj.TempMin = Convert.ToString(weatherInfo.main.temp_min);
                WeatherObj.WeatherIcon = weatherInfo.weather[0].icon;

            }
            return View();
        }
    }
}
