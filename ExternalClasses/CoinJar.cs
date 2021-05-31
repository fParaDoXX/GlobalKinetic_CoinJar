using GlobalKinetic_CoinJar.Data;
using GlobalKinetic_CoinJar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalKinetic_CoinJar.ExternalClasses
{
    public class CoinJar : ICoinJar
    {
        public void AddCoin(ModelView model, SysContext _context)
        {

            var newestCoinJar = _context.CoinJars.OrderByDescending(p => p.ID).FirstOrDefault();
            var coinInfo = _context.Coins.Where(x => x.ID == model.CoinJars.CoinID).FirstOrDefault();
            var coinJarVolume = 0.00m;

            if (newestCoinJar != null)
            {
                coinJarVolume = coinInfo.Volume + GetTotalVolume(newestCoinJar.ID, _context);

                CoinJars coinJar = new CoinJars
                {
                    CoinID = model.CoinJars.CoinID,
                    CurrentValue = coinInfo.Amount + GetTotalAmount(newestCoinJar.ID, _context),
                    ResetCounter = newestCoinJar.ResetCounter,
                    CurrentVolume = coinJarVolume,
                    IsActive = true,
                    CreatedDate = DateTime.Today
                };

                _context.Add(coinJar);
                _context.SaveChanges();
            }
            else
            {
                CoinJars coinJar = new CoinJars
                {
                    CoinID = model.CoinJars.CoinID,
                    CurrentValue = model.CoinJars.CurrentValue,
                    ResetCounter = 0,
                    CurrentVolume = coinJarVolume,
                    IsActive = true,
                    CreatedDate = DateTime.Today
                };

                _context.Add(coinJar);
                _context.SaveChanges();
            }
        }

        public decimal GetTotalVolume(long CoinJarID, SysContext _context)
        {
            var CoinJarVolume = _context.CoinJars.Where(x => x.ID == CoinJarID).FirstOrDefault();
            return CoinJarVolume.CurrentVolume;

        }

        public decimal GetTotalAmount(long CoinJarID, SysContext _context)
        {
            var CoinJarValue = _context.CoinJars.Where(x => x.ID == CoinJarID).FirstOrDefault();
            return CoinJarValue.CurrentValue;
        }

        public void Reset(SysContext _context)
        {
            var newestCoinJar = _context.CoinJars.OrderByDescending(p => p.ID).FirstOrDefault();
            long resetCounter = 1;
            if (newestCoinJar != null)
            {
                resetCounter += newestCoinJar.ResetCounter;
            }

            CoinJars coinJar = new CoinJars
            {
                CoinID = 0,
                CurrentValue = 0,
                ResetCounter = 0,
                CurrentVolume = resetCounter,
                IsActive = true,
                CreatedDate = DateTime.Today
            };

            _context.Add(coinJar);
            _context.SaveChanges();
        }
    }
}
