using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApplication.Models
{
    public class WeatherViewModel
    {
        public String Country { get; set; }
        public String City { get; set; }
        public String Humidity { get; set; }
        public String Temp { get; set; }
        public String TempMin { get; set; }
        public String TempFeelsLike { get; set; }
        public String WeatherCondition { get; set; }
        public String TempMax { get; set; }
        public String Sunrise { get; set; }
        public String Sunset { get; set; }
        public String WindSpeed { get; set; }
        public String WindDirection { get; set; }
        public String RainProbabilty { get; set; }
        public String Date { get; set; }
        public String Day { get; set; }
        public String WordDate { get; set; }


    }
    public class RootObject
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public Forecast forecast { get; set; }
    }

    public class Location
    {
        public string name { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string tz_id { get; set; }
        public int localtime_epoch { get; set; }
        public string localtime { get; set; }
    }

    public class Current
    {
        public int last_updated_epoch { get; set; }
        public string last_updated { get; set; }
        public float temp_c { get; set; }
        public float temp_f { get; set; }
        public int is_day { get; set; }
        public Condition condition { get; set; }
        public float wind_mph { get; set; }
        public float wind_kph { get; set; }
        public int wind_degree { get; set; }
        public string wind_dir { get; set; }
        public float pressure_mb { get; set; }
        public float pressure_in { get; set; }
        public float precip_mm { get; set; }
        public float precip_in { get; set; }
        public int humidity { get; set; }
        public int cloud { get; set; }
        public float feelslike_c { get; set; }
        public float feelslike_f { get; set; }
        public float vis_km { get; set; }
        public float vis_miles { get; set; }
        public float uv { get; set; }
        public float gust_mph { get; set; }
        public float gust_kph { get; set; }
    }

    public class Condition
    {
        public string text { get; set; }
        public string icon { get; set; }
        public int code { get; set; }
    }

    public class Forecast
    {
        public Forecastday[] forecastday { get; set; }
    }

    public class Forecastday
    {
        public string date { get; set; }
        public int date_epoch { get; set; }
        public Day day { get; set; }
        public Astro astro { get; set; }
        public Hour[] hour { get; set; }
    }

    public class Day
    {
        public float maxtemp_c { get; set; }
        public float maxtemp_f { get; set; }
        public float mintemp_c { get; set; }
        public float mintemp_f { get; set; }
        public float avgtemp_c { get; set; }
        public float avgtemp_f { get; set; }
        public float maxwind_mph { get; set; }
        public float maxwind_kph { get; set; }
        public float totalprecip_mm { get; set; }
        public float totalprecip_in { get; set; }
        public float avgvis_km { get; set; }
        public float avgvis_miles { get; set; }
        public float avghumidity { get; set; }
        public int daily_will_it_rain { get; set; }
        public string daily_chance_of_rain { get; set; }
        public int daily_will_it_snow { get; set; }
        public string daily_chance_of_snow { get; set; }
        public Condition1 condition { get; set; }
        public float uv { get; set; }
    }

    public class Condition1
    {
        public string text { get; set; }
        public string icon { get; set; }
        public int code { get; set; }
    }

    public class Astro
    {
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string moonrise { get; set; }
        public string moonset { get; set; }
        public string moon_phase { get; set; }
        public string moon_illumination { get; set; }
    }

    public class Hour
    {
        public int time_epoch { get; set; }
        public string time { get; set; }
        public float temp_c { get; set; }
        public float temp_f { get; set; }
        public int is_day { get; set; }
        public Condition2 condition { get; set; }
        public float wind_mph { get; set; }
        public float wind_kph { get; set; }
        public int wind_degree { get; set; }
        public string wind_dir { get; set; }
        public float pressure_mb { get; set; }
        public float pressure_in { get; set; }
        public float precip_mm { get; set; }
        public float precip_in { get; set; }
        public int humidity { get; set; }
        public int cloud { get; set; }
        public float feelslike_c { get; set; }
        public float feelslike_f { get; set; }
        public float windchill_c { get; set; }
        public float windchill_f { get; set; }
        public float heatindex_c { get; set; }
        public float heatindex_f { get; set; }
        public float dewpoint_c { get; set; }
        public float dewpoint_f { get; set; }
        public int will_it_rain { get; set; }
        public string chance_of_rain { get; set; }
        public int will_it_snow { get; set; }
        public string chance_of_snow { get; set; }
        public float vis_km { get; set; }
        public float vis_miles { get; set; }
        public float gust_mph { get; set; }
        public float gust_kph { get; set; }
        public float uv { get; set; }
    }

    public class Condition2
    {
        public string text { get; set; }
        public string icon { get; set; }
        public int code { get; set; }
    }

}