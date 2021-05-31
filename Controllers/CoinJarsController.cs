using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlobalKinetic_CoinJar.Data;
using GlobalKinetic_CoinJar.Models;
using GlobalKinetic_CoinJar.ExternalClasses;
using System.Web.Helpers;

namespace GlobalKinetic_CoinJar.Controllers
{
    public class CoinJarsController : Controller
    {
        private readonly SysContext _context;
        private readonly ICoinJar _CoinJar;

        public CoinJarsController(SysContext context,ICoinJar coinJar)
        {
            _context = context;
            _CoinJar = coinJar;
        }

        // GET: CoinJars
        public async Task<IActionResult> Index()
        {
            var lstCoinJars = await _context.CoinJars.ToListAsync();

            ModelView modelView = new ModelView
            {
                CoinJarsIE = lstCoinJars
            };

            return View(modelView);
        }

        // GET: CoinJars/Create
        public IActionResult Create()
        {
            var lstOfCoins = (from a in _context.Coins
                                  where a.IsActive == true

                                  select new SelectListItem { Value = a.ID.ToString(), Text = a.Name.ToUpper() }).ToList();
            var newestCoinJar = _context.CoinJars.OrderByDescending(p => p.ID).FirstOrDefault();
            var coinJarValue = 0.00m;
            var coinJarVolume = 0.00m;
            if (newestCoinJar != null)
            {
                coinJarValue = newestCoinJar.CurrentValue;
                coinJarVolume = newestCoinJar.CurrentVolume;
            }

            CoinJars coinJars = new CoinJars
            {
                CoinsSl = lstOfCoins,
                CurrentValue = coinJarValue,
                CurrentVolume = coinJarVolume
            };

            ModelView modelView = new ModelView
            {
                CoinJars = coinJars
            };

            return View(modelView);
        }

        // POST: CoinJars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModelView model)
        {
            if (ModelState.IsValid)
            {
                var newestCoinJar = _context.CoinJars.OrderByDescending(p => p.ID).FirstOrDefault();
                var coinInfo = _context.Coins.Where(x => x.ID == model.CoinJars.CoinID).FirstOrDefault();
                decimal NewCoinJarVolume = 0.00m; 
                if (newestCoinJar!= null)
                {
                    NewCoinJarVolume= coinInfo.Volume +  _CoinJar.GetTotalVolume(newestCoinJar.ID, _context);
                }
               

                if (NewCoinJarVolume >= 42)
                {
                    ModelState.AddModelError("Volume", "Coin Jar does't have enough space for this coin, Please Rest Coin Jar");
                    return View(model);
                }

                model.CoinJars.CurrentVolume = NewCoinJarVolume;

                _CoinJar.AddCoin(model, _context);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: CoinJars/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coinJars = await _context.CoinJars.FindAsync(id);
            if (coinJars == null)
            {
                return NotFound();
            }
            return View(coinJars);
        }

        // POST: CoinJars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,CurrentValue,CurrentVolume,ResetCounter,CoinID,IsActive,CreatedDate,ResetDate")] CoinJars coinJars)
        {
            if (id != coinJars.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coinJars);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoinJarsExists(coinJars.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coinJars);
        }

        // GET: CoinJars/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coinJars = await _context.CoinJars
                .FirstOrDefaultAsync(m => m.ID == id);
            if (coinJars == null)
            {
                return NotFound();
            }

            return View(coinJars);
        }

        // POST: CoinJars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var coinJars = await _context.CoinJars.FindAsync(id);
            _context.CoinJars.Remove(coinJars);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Reset() 
        {
            _CoinJar.Reset(_context);

            return RedirectToAction(nameof(Create));
        }


        private bool CoinJarsExists(long id)
        {
            return _context.CoinJars.Any(e => e.ID == id);
        }


        public JsonResult GetCoinData(long Id)
        {
            var CoinData = _context.Coins.Where(x => x.ID == Id).FirstOrDefault();
            //return Json(new
            //{
            //    Name = CoinData.Name,
            //    Amount = CoinData.Amount,
            //    Volume = CoinData.Volume
            //});


            //var returnValue = Json(new
            //{
            //    Name = "Test",
            //    Amount = CoinData.Amount,
            //    Volume = CoinData.Volume
            //});
            return Json(CoinData);
        }
    }
}
