using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalKinetic_CoinJar.Models
{
    public class ModelView
    {
        public virtual CoinJars CoinJars { get; set; }
        public virtual IEnumerable<CoinJars> CoinJarsIE { get; set; }

        public virtual Coins Coins { get; set; }
        public virtual IEnumerable<Coins> CoinsIE { get; set; } 

    }
}
