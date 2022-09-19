using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BestellserviceWeb.Data;
using BestellserviceWeb.Models;

namespace BestellserviceWeb.Controllers
{
    public class TblLeistungsbilderProjektsController : Controller
    {
        private readonly BestellserviceDBContext _context;

        public TblLeistungsbilderProjektsController(BestellserviceDBContext context)
        {
            _context = context;
        }

        // GET: TblLeistungsbilderProjekts
        public async Task<IActionResult> Index()
        {
            var bestellserviceDBContext = _context.TblLeistungsbilderProjekt.Include(t => t.LeistpLeistungsbildNavigation).Include(t => t.LeistpProjektNavigation);
            return View(await bestellserviceDBContext.ToListAsync());
        }

        // GET: TblLeistungsbilderProjekts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblLeistungsbilderProjekt = await _context.TblLeistungsbilderProjekt
                .Include(t => t.LeistpLeistungsbildNavigation)
                .Include(t => t.LeistpProjektNavigation)
                .FirstOrDefaultAsync(m => m.LeistpId == id);
            if (tblLeistungsbilderProjekt == null)
            {
                return NotFound();
            }

            return View(tblLeistungsbilderProjekt);
        }

        // GET: TblLeistungsbilderProjekts/Create
        public IActionResult Create()
        {
            List<TblLeistungsbilder> leistungsbilder = _context.TblLeistungsbilder.ToList();
            List<TblLeistungsbilder> leistungsbilderEdited = leistungsbilder.ConvertAll(s => new TblLeistungsbilder { LeistId = s.LeistId, LeistName = s.LeistName, LeistParent = s.LeistParent }).ToList();
            List<int> depthList = new List<int>();
            LeistungsbilderProjektList listWithAll = new LeistungsbilderProjektList();
            

            int depth =0;
            int counter = 0;
            foreach (TblLeistungsbilder bild in leistungsbilderEdited)
            {
                depth = 0;
                TblLeistungsbilder tempLeistungsbild = leistungsbilder[counter];

                while (tempLeistungsbild.LeistParent != null)
                {
                    for (int i = 0; i < leistungsbilder.Count(); i++)
                    {
                        if (tempLeistungsbild.LeistParent == leistungsbilder[i].LeistId)
                        {
                            bild.LeistName = bild.LeistName + " <- " + leistungsbilder[i].LeistName;
                            tempLeistungsbild = leistungsbilder[i];
                            depth++;
                            break;
                        }
                    }
                }
                depthList.Add(depth);
                counter++;
            }
            ViewData["Depth"] = depthList;
            ViewData["LeistpLeistungsbild"] = new SelectList(leistungsbilderEdited, "LeistId", "LeistName");
            ViewData["LeistpProjekt"] = new SelectList(_context.TblProjekt, "ProjId", "ProjName");

            listWithAll.LeistungsbildDepth = depthList;
            listWithAll.Leistungsbild = leistungsbilderEdited;
            for(int i = 0; i < depthList.Count; i++)
            {
                listWithAll.MoneyAmount.Add(0);
            }

            for (int i = 0; i < depthList.Count; i++)
            {
                listWithAll.LeistungsbildActive.Add(false);
            }

            return View(listWithAll);
        }

        // POST: TblLeistungsbilderProjekts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeistpId,LeistpProjekt,LeistpLeistungsbild,LeistpAmount")] TblLeistungsbilderProjekt tblLeistungsbilderProjekt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblLeistungsbilderProjekt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LeistpLeistungsbild"] = new SelectList(_context.TblLeistungsbilder, "LeistId", "LeistName", tblLeistungsbilderProjekt.LeistpLeistungsbild);
            ViewData["LeistpProjekt"] = new SelectList(_context.TblProjekt, "ProjId", "ProjName", tblLeistungsbilderProjekt.LeistpProjekt);
            return View(tblLeistungsbilderProjekt);
        }

        // GET: TblLeistungsbilderProjekts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblLeistungsbilderProjekt = await _context.TblLeistungsbilderProjekt.FindAsync(id);
            if (tblLeistungsbilderProjekt == null)
            {
                return NotFound();
            }
            ViewData["LeistpLeistungsbild"] = new SelectList(_context.TblLeistungsbilder, "LeistId", "LeistName", tblLeistungsbilderProjekt.LeistpLeistungsbild);
            ViewData["LeistpProjekt"] = new SelectList(_context.TblProjekt, "ProjId", "ProjName", tblLeistungsbilderProjekt.LeistpProjekt);
            return View(tblLeistungsbilderProjekt);
        }

        // POST: TblLeistungsbilderProjekts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeistpId,LeistpProjekt,LeistpLeistungsbild,LeistpAmount")] TblLeistungsbilderProjekt tblLeistungsbilderProjekt)
        {
            if (id != tblLeistungsbilderProjekt.LeistpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblLeistungsbilderProjekt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblLeistungsbilderProjektExists(tblLeistungsbilderProjekt.LeistpId))
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
            ViewData["LeistpLeistungsbild"] = new SelectList(_context.TblLeistungsbilder, "LeistId", "LeistName", tblLeistungsbilderProjekt.LeistpLeistungsbild);
            ViewData["LeistpProjekt"] = new SelectList(_context.TblProjekt, "ProjId", "ProjName", tblLeistungsbilderProjekt.LeistpProjekt);
            return View(tblLeistungsbilderProjekt);
        }

        // GET: TblLeistungsbilderProjekts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblLeistungsbilderProjekt = await _context.TblLeistungsbilderProjekt
                .Include(t => t.LeistpLeistungsbildNavigation)
                .Include(t => t.LeistpProjektNavigation)
                .FirstOrDefaultAsync(m => m.LeistpId == id);
            if (tblLeistungsbilderProjekt == null)
            {
                return NotFound();
            }

            return View(tblLeistungsbilderProjekt);
        }

        // POST: TblLeistungsbilderProjekts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblLeistungsbilderProjekt = await _context.TblLeistungsbilderProjekt.FindAsync(id);
            _context.TblLeistungsbilderProjekt.Remove(tblLeistungsbilderProjekt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblLeistungsbilderProjektExists(int id)
        {
            return _context.TblLeistungsbilderProjekt.Any(e => e.LeistpId == id);
        }
    }
}
