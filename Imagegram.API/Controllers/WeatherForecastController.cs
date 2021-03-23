using Imagegram.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imagegram.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get([FromHeader(Name = "X-Account-Id")] string accountId)
        {
            return await Mediator.Send(new GetWeatherForecastsQuery());
        }
    }
}