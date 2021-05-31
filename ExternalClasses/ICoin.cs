using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalKinetic_CoinJar.ExternalClasses
{
    public interface ICoin
    {
        decimal Amount { get; set; }
        decimal Volume { get; set; }

      
    }
}
