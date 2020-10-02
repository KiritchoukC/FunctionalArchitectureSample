using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Architecture.Application.WeatherForecast;
using Architecture.Application.WeatherForecast.Queries.GetAllWeatherForecasts;
using Architecture.Application.WeatherForecast.Queries.GetSomeWeatherForecasts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Architecture.Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/weather-forecasts")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherForecastController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<WeatherForecast>> GetAll()
        {
            return await _mediator.Send(new GetAllWeatherForecastsQuery());
        }

        [HttpGet("some")]
        public async Task<IEnumerable<WeatherForecast>> GetSome([FromQuery] GetSomeWeatherForecastsQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}