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

                WeatherViewModel WeatherObj = new WeatherViewModel
                {
                    Country = weatherInfo.location.country,
                    City = weatherInfo.location.name,
                    WeatherCondition = weatherInfo.forecast.forecastday[0].day.condition.text,
                    Humidity = Convert.ToString(weatherInfo.forecast.forecastday[0].day.avghumidity),
                    Temp = Convert.ToString(weatherInfo.forecast.forecastday[0].day.avgtemp_c),
                    TempFeelsLike = Convert.ToString(weatherInfo.forecast.forecastday[0].hour[0].feelslike_c),
                    TempMax = Convert.ToString(weatherInfo.forecast.forecastday[0].day.maxtemp_c),
                    TempMin = Convert.ToString(weatherInfo.forecast.forecastday[0].day.mintemp_c),
                    Sunrise = weatherInfo.forecast.forecastday[0].astro.sunrise,
                    Sunset = weatherInfo.forecast.forecastday[0].astro.sunset,
                    WindDirection = weatherInfo.forecast.forecastday[0].hour[0].wind_dir,
                    WindSpeed = Convert.ToString(weatherInfo.forecast.forecastday[0].hour[0].wind_kph),
                    RainProbabilty = Convert.ToString(weatherInfo.forecast.forecastday[0].day.daily_chance_of_rain)
                };

            }
            return View();
        }
    }
}
