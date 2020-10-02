using System;

namespace Architecture.Application.WeatherForecast
{
    public class WeatherForecast
    {
        private WeatherForecast(DateTime date, int temperatureC, string summary)
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }

        public static WeatherForecast CreateInstance(DateTime date, int temperatureC, string summary)
        {
            return new WeatherForecast(date, temperatureC, summary);
        }

        public DateTime Date { get; }

        public int TemperatureC { get; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public string Summary { get; }
    }
}