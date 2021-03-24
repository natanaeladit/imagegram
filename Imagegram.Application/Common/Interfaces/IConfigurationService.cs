using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagegram.Application.Common.Interfaces
{
    public interface IConfigurationService
    {
        string GetConfig(string key);
    }
}
