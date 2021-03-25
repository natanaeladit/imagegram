using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagegram.API;

namespace Imagegram.IntegrationTests
{
    public class ApiApplicationFactory : WebApplicationFactory<Startup>
    {
    }
}
