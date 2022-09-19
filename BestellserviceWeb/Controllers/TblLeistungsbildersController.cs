using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BestellserviceWeb.Data;

namespace BestellserviceWeb.Models
{
    public class TblLeistungsbildersController : Controller
    {
        private readonly BestellserviceDBContext _context;

        public TblLeistungsbildersController(BestellserviceDBContext context)
        {
            _context = context;
        }

        // GET: TblLeistungsbilders
        public async Task<IActionResult> Index()
        {
            return View(await _context.TblLeistungsbilder.ToListAsync());
        }

        // GET: TblLeistungsbilders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblLeistungsbilder = await _context.TblLeistungsbilder
                .FirstOrDefaultAsync(m => m.LeistId == id);
            if (tblLeistungsbilder == null)
            {
                return NotFound();
            }


            List<TblLeistungsbilder> leistungsbilder = _context.TblLeistungsbilder.ToList();
            List<TblLeistungsbilder> leistungsbilderNeeded = new List<TblLeistungsbilder>();


                TblLeistungsbilder tempLeistungsbild = tblLeistungsbilder;

                while (tempLeistungsbild.LeistParent != null)
                {
                    for (int i = 0; i < leistungsbilder.Count(); i++)
                    {
                        if (tempLeistungsbild.LeistParent == leistungsbilder[i].LeistId)
                        {
                            leistungsbilderNeeded.Add(leistungsbilder[i]);
                            tempLeistungsbild = leistungsbilder[i];
                            break;
                        }
                    }
                }

            ViewBag.leistungsbilderParents = leistungsbilderNeeded;


            return View(tblLeistungsbilder);
        }

        // GET: TblLeistungsbilders/Create
        public IActionResult Create()
        {
            List<TblLeistungsbilder> leistungsbilder = _context.TblLeistungsbilder.ToList();
            List<TblLeistungsbilder> leistungsbilderEdited = leistungsbilder.ConvertAll(s => new TblLeistungsbilder {LeistId = s.LeistId, LeistName = s.LeistName, LeistParent = s.LeistParent}).ToList();


            int counter = 0;
            foreach (TblLeistungsbilder bild in leistungsbilderEdited)
            {
                TblLeistungsbilder tempLeistungsbild = leistungsbilder[counter];

                while (tempLeistungsbild.LeistParent != null)
                {
                    for (int i = 0; i < leistungsbilder.Count(); i++)
                    {
                        if (tempLeistungsbild.LeistParent == leistungsbilder[i].LeistId)
                        {
                            bild.LeistName = bild.LeistName  + " <- " + leistungsbilder[i].LeistName;
                            tempLeistungsbild = leistungsbilder[i];
                            break;
                        }
                    }
                }
                counter++;
            }

            ViewBag.leistungsbilder = leistungsbilderEdited;
            return View();
        }

        // POST: TblLeistungsbilders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeistId,LeistParent,LeistName")] TblLeistungsbilder tblLeistungsbilder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblLeistungsbilder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblLeistungsbilder);
        }

        // GET: TblLeistungsbilders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblLeistungsbilder = await _context.TblLeistungsbilder.FindAsync(id);
            if (tblLeistungsbilder == null)
            {
                return NotFound();
            }
            return View(tblLeistungsbilder);
        }

        // POST: TblLeistungsbilders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeistId,LeistParent,LeistName")] TblLeistungsbilder tblLeistungsbilder)
        {
            if (id != tblLeistungsbilder.LeistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblLeistungsbilder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblLeistungsbilderExists(tblLeistungsbilder.LeistId))
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
            return View(tblLeistungsbilder);
        }

        // GET: TblLeistungsbilders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblLeistungsbilder = await _context.TblLeistungsbilder
                .FirstOrDefaultAsync(m => m.LeistId == id);
            if (tblLeistungsbilder == null)
            {
                return NotFound();
            }

            return View(tblLeistungsbilder);
        }

        // POST: TblLeistungsbilders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblLeistungsbilder = await _context.TblLeistungsbilder.FindAsync(id);
            _context.TblLeistungsbilder.Remove(tblLeistungsbilder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblLeistungsbilderExists(int id)
        {
            return _context.TblLeistungsbilder.Any(e => e.LeistId == id);
        }
    }
}
