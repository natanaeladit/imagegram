using Imagegram.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagegram.Infrastructure.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;
        public ConfigurationService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConfig(string key)
        {
            return _configuration[key];
        }
    }
}
