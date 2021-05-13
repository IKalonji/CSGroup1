using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherApplication.Data;
using WeatherApplication.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WeatherApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Inside one of your controller actions
            if (User.Identity.IsAuthenticated)
            {
                string idToken = User.Claims.First()?.Value;
                Console.WriteLine(idToken);
                User user = _context.Users.FirstOrDefault(u => u.AccessToken == idToken);
                
                // if the user id_token doesn't exist, create it  
                if (user == null)
                {
                    user = _context.Users.Add(entity: new User() { AccessToken = idToken }).Entity;
                    await _context.SaveChangesAsync();
                }
                ViewData["UserId"] = $"{user.Id}";
            }
            ViewData["Title"] = "Your Weather";
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Home()
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
            string appId = "adff024fa02240e281970159211105";

            //API path with CITY parameter and other parameters.  
            string url = string.Format("http://api.weatherapi.com/v1/forecast.json?key={0}&q={1}&days=3&hour=12", appId, City);

            using (System.Net.WebClient client = new WebClient())
            {
                string json = client.DownloadString(url); 
        
                RootObject weatherInfo = JsonConvert.DeserializeObject<RootObject>(json);
                List<WeatherViewModel> ForecastList = new List<WeatherViewModel>();
                for (int i = 0; i <= 2; i++) {
                    WeatherViewModel WeatherObj = new WeatherViewModel
                    {
                        Country = weatherInfo.location.country,
                        City = weatherInfo.location.name,
                        WeatherCondition = weatherInfo.forecast.forecastday[i].day.condition.text,
                        Humidity = Convert.ToString(weatherInfo.forecast.forecastday[i].day.avghumidity),
                        Temp = Convert.ToString(weatherInfo.forecast.forecastday[i].day.avgtemp_c),
                        TempFeelsLike = Convert.ToString(weatherInfo.forecast.forecastday[i].hour[0].feelslike_c),
                        TempMax = Convert.ToString(weatherInfo.forecast.forecastday[i].day.maxtemp_c),
                        TempMin = Convert.ToString(weatherInfo.forecast.forecastday[i].day.mintemp_c),
                        Sunrise = weatherInfo.forecast.forecastday[i].astro.sunrise,
                        Sunset = weatherInfo.forecast.forecastday[i].astro.sunset,
                        WindDirection = weatherInfo.forecast.forecastday[i].hour[0].wind_dir,
                        WindSpeed = Convert.ToString(weatherInfo.forecast.forecastday[i].hour[0].wind_kph),
                        RainProbabilty = Convert.ToString(weatherInfo.forecast.forecastday[i].day.daily_chance_of_rain),
                        Date = weatherInfo.forecast.forecastday[i].date,
                        Day = ConvertToDay(weatherInfo.forecast.forecastday[i].date,false),
                        WordDate = ConvertToDay(weatherInfo.forecast.forecastday[i].date,true)
                    };
                    ForecastList.Add(WeatherObj);
                    
                }
                ViewBag.Forecast = ForecastList;
            }
            return View();
        }

        public String ConvertToDay(string DateString, bool Word)
        {
            DateTime dateValue;
            DateTimeOffset dateOffsetValue;
            dateValue = DateTime.Parse(DateString, CultureInfo.InvariantCulture);
            dateOffsetValue = new DateTimeOffset(dateValue,
                                      TimeZoneInfo.Local.GetUtcOffset(dateValue));
            dateValue = DateTime.Parse(DateString, CultureInfo.InvariantCulture);
            if (Word)
            {
                return dateValue.ToString("ddd d MMM");
            }
                return dateValue.ToString("dddd");

        }
    }
}
