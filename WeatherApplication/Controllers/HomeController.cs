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
            ViewData["Title"] = "Home Page - C# Weather";

            var queryString = Request.Query.ToDictionary(q => q.Key, q => q.Value);

            string City = "Cape Town";

            try{
                City = queryString["City"];
            } catch {
                City = "Cape Town";
            }
            

            // setupWeatherAPI("Cape Town");
            setupWeatherAPI(City);
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


        [HttpPost]
        public IActionResult Index(string City)
        {
            ViewData["Title"] = "Home Page - C# Weather";
            setupWeatherAPI(City);
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

        public void setupWeatherAPI(string City) {
            string appId = "adff024fa02240e281970159211105";
            //API path with CITY parameter and other parameters.  
            string url = string.Format("http://api.weatherapi.com/v1/forecast.json?key={0}&q={1}&days=3&hour=12", appId, City);
            
            using (System.Net.WebClient client = new WebClient())
            {
                string json = client.DownloadString(url); 
        
                RootObject weatherInfo = JsonConvert.DeserializeObject<RootObject>(json);
                List<WeatherViewModel> ForecastList = new List<WeatherViewModel>();
                String Hot = "Cold";
                if (weatherInfo.forecast.forecastday[0].day.avgtemp_c >= 20)
                {
                    Hot = "Hot";
                   }
                else if (weatherInfo.forecast.forecastday[0].day.avgtemp_c <= 5) {
                    Hot = "Freezing";
                }
                for (int i = 0; i <= 2; i++) {
                    WeatherViewModel WeatherObj = new WeatherViewModel
                    {
                        Country = weatherInfo.location.country,
                        City = weatherInfo.location.name,
                        WeatherCondition = weatherInfo.forecast.forecastday[i].day.condition.text,
                        Humidity = Convert.ToString(weatherInfo.forecast.forecastday[i].day.avghumidity),
                        Temp = Convert.ToString(Convert.ToInt32(weatherInfo.forecast.forecastday[i].day.avgtemp_c)),
                        TempFeelsLike = Convert.ToString(Convert.ToInt32(weatherInfo.forecast.forecastday[i].hour[0].feelslike_c)),
                        TempMax = Convert.ToString(Convert.ToInt32(weatherInfo.forecast.forecastday[i].day.maxtemp_c)),
                        TempMin = Convert.ToString(Convert.ToInt32(weatherInfo.forecast.forecastday[i].day.mintemp_c)),
                        Sunrise = weatherInfo.forecast.forecastday[i].astro.sunrise,
                        Sunset = weatherInfo.forecast.forecastday[i].astro.sunset,
                        WindDirection = weatherInfo.forecast.forecastday[i].hour[0].wind_dir,
                        WindSpeed = Convert.ToString(weatherInfo.forecast.forecastday[i].hour[0].wind_kph),
                        RainProbabilty = Convert.ToString(weatherInfo.forecast.forecastday[i].day.daily_chance_of_rain),
                        Hot = Hot
                        Day = this.ConvertToDay(weatherInfo.forecast.forecastday[i].date,false),
                        WordDate = this.ConvertToDay(weatherInfo.forecast.forecastday[i].date,true),
                        Icon = weatherInfo.forecast.forecastday[i].day.condition.icon
                    };
                    ForecastList.Add(WeatherObj);
                }
                setWeatherForDay(ForecastList);
                
            }
        }

        public void setWeatherForDay(List<WeatherViewModel> ForecastList) {
            ViewData["Day 1 Temp"] = ForecastList[0].Temp;
            ViewData["City"] = ForecastList[0].City;
            ViewData["WeatherCondition"] = ForecastList[0].WeatherCondition;
            ViewData["Humidity"] = ForecastList[0].Humidity;
            ViewData["TempMax"] = ForecastList[0].TempMax;
            ViewData["TempMin"] = ForecastList[0].TempMin;
            ViewData["Sunrise"] = ForecastList[0].Sunrise;
            ViewData["Sunset"] = ForecastList[0].Sunset;
            ViewData["WindDirection"] = ForecastList[0].WindDirection;
            ViewData["WindSpeed"] = ForecastList[0].WindSpeed;
            ViewData["RainProbability"] = ForecastList[0].RainProbabilty;
            ViewData["Day 2 Max Temp"] = ForecastList[1].TempMax;
            ViewData["Day 2 Min Temp"] = ForecastList[1].TempMin;
            ViewData["Day 3 Max Temp"] = ForecastList[2].TempMax;
            ViewData["Day 3 Min Temp"] = ForecastList[2].TempMin;
            ViewData["Day 1 Icon"] = ForecastList[0].Icon;
            ViewData["Day 2 Icon"] = ForecastList[1].Icon;
            ViewData["Day 3 Icon"] = ForecastList[2].Icon;
            
            if (ForecastList[0].Hot == "Hot") {
                ViewData["Clothing"] = "https://www.sportscene.co.za/plp/men/clothing/t-shirts/_/N-280i#p=1&e=280iZ8s3hdu&f=sku.activePrice%257CBTWN+69+2500";
            }
            else if(ForecastList[0].Hot == "Freezing")
            {
                ViewData["Clothing"] = "https://www.sportscene.co.za/plp/women/clothing/jackets/_/N-2851;jsessionid=sUNVN595HPFYyedCipmkEOYulyoTSVi2NC0l5SEt.tfg-prd-com-85#p=1&e=2851Z8s3hdu&f=sku.activePrice%257CBTWN+219+2500";
            }
            else
            {
                ViewData["Clothing"] = "https://www.sportscene.co.za/plp/men/clothing/sweats-hoodies/_/N-280k;jsessionid=1c1rh_DpsfrNCGY1EysqKu4LbzCXDm7HDbp69br_.tfg-prd-com-85#p=1&e=280kZ8s3hdu&f=sku.activePrice%257CBTWN+249+1700";
            }

            ViewData["Day 1 Day"] = ForecastList[0].Day;
            ViewData["Day 2 Day"] = ForecastList[1].Day;
            ViewData["Day 3 Day"] = ForecastList[2].Day;
            ViewData["Day 1 WordDate"] = ForecastList[0].WordDate;
            ViewData["Day 2 WordDate"] = ForecastList[1].WordDate;
            ViewData["Day 3 WordDate"] = ForecastList[2].WordDate;
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult AddRegions()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateRegion(string? region) 
        {
            return RedirectToAction("Index");
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
                return dateValue.ToString("d MMM");
            }
                return dateValue.ToString("dddd");

        }
    }
}
