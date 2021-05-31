using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlobalKinetic_CoinJar.Data;
using GlobalKinetic_CoinJar.Models;

namespace GlobalKinetic_CoinJar.Controllers
{
    public class CoinsController : Controller
    {
        private readonly SysContext _context;

        private const decimal ConversionConstant = 0.554113m;

        public CoinsController(SysContext context)
        {
            _context = context;
        }

        // GET: Coins
        public async Task<IActionResult> Index()
        {
            var lstCoins = await _context.Coins.ToListAsync();

            ModelView modelView = new ModelView
            {
                CoinsIE = lstCoins
            };

            return View(modelView);
        }

        // GET: Coins/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coins = await _context.Coins
                .FirstOrDefaultAsync(m => m.ID == id);
            if (coins == null)
            {
                return NotFound();
            }

            return View(coins);
        }

        // GET: Coins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModelView model)
        {
            if (ModelState.IsValid)
            {


                Coins coins = new Coins
                {
                    Name = model.Coins.Name,
                    Amount = model.Coins.Amount,
                    Volume = ConvertIn3ToFlOz(model.Coins.Volume),
                    IsActive = true,
                    CreatedDate = DateTime.Today
                };

                _context.Add(coins);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Coins/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coinInfo = await _context.Coins.FindAsync(id);
            if (coinInfo == null)
            {
                return NotFound();
            }

            ModelView modelView = new ModelView
            {
                Coins = coinInfo
            };

            ViewBag.ID = id;

            return View(modelView);
        }

        // POST: Coins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ModelView model)
        {
            if (id != model.Coins.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.Coins.ModifiedDate = DateTime.Today;
                    _context.Update(model.Coins);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoinsExists(model.Coins.ID))
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
            return View(model);
        }

        // GET: CoinJars/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coin = await _context.Coins
                .FirstOrDefaultAsync(m => m.ID == id);
            if (coin == null)
            {
                return NotFound();
            }

            ModelView modelView = new ModelView
            {
                Coins = coin
            };

            return View(modelView);
        }

        // POST: CoinJars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var coin = await _context.Coins.FindAsync(id);
            _context.Coins.Remove(coin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        #region Functions
        private bool CoinsExists(long id)
        {
            return _context.Coins.Any(e => e.ID == id);
        }


        private decimal ConvertIn3ToFlOz(decimal Volume)
        {
            return decimal.Multiply(Volume, ConversionConstant);
        }
        #endregion
    }
}
