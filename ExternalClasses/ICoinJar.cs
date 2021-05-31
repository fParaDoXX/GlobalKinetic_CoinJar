using GlobalKinetic_CoinJar.Data;
using GlobalKinetic_CoinJar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalKinetic_CoinJar.ExternalClasses
{
    public interface ICoinJar
    {
        void AddCoin(ModelView model, SysContext _context);
        decimal GetTotalVolume(long CoinJarID, SysContext _context);
        decimal GetTotalAmount(long CoinJarID, SysContext _context);
        void Reset(SysContext _context);

    }
}
