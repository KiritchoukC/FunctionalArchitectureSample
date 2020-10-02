using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Architecture.Application.WeatherForecast.Queries.GetAllWeatherForecasts
{
    public class GetAllWeatherForecastsQuery : IRequest<ImmutableList<WeatherForecast>> { }

    public class GetAllWeatherForecastsQueryHandler : IRequestHandler<GetAllWeatherForecastsQuery, ImmutableList<WeatherForecast>>
    {
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<ImmutableList<WeatherForecast>> Handle(GetAllWeatherForecastsQuery query,
            CancellationToken cancellationToken)
        {
            var rng = new Random();
            var forecasts =
                Enumerable
                    .Range(1, 5)
                    .Select(index =>
                    {
                        var forecast = WeatherForecast.CreateInstance(
                            DateTime.Now.AddDays(index),
                            rng.Next(-20, 55),
                            Summaries[rng.Next(Summaries.Length)]);
                        return forecast;
                    })
                    .ToImmutableList();

            return Task.FromResult(forecasts);
        }
    }
}