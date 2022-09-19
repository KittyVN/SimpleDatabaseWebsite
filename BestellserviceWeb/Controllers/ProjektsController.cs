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
    public class ProjektsController : Controller
    {
        private readonly BestellserviceDBContext _context;

        public ProjektsController(BestellserviceDBContext context)
        {
            _context = context;
        }

        // GET: Projekts
        public async Task<IActionResult> Index()
        {
            return View(await _context.TblProjekt.ToListAsync());
        }

        // GET: Projekts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblProjekt = await _context.TblProjekt
                .FirstOrDefaultAsync(m => m.ProjId == id);
            if (tblProjekt == null)
            {
                return NotFound();
            }

            var bestellserviceDBContext = await _context.TblLeistungsbilderProjekt.Include(t => t.LeistpLeistungsbildNavigation).Include(t => t.LeistpProjektNavigation).Where(i => i.LeistpProjekt == id).ToListAsync();
            ViewData["Leistungsbilder"] = bestellserviceDBContext;


            return View(tblProjekt);
        }

        // GET: Projekts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projekts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjId,ProjName")] TblProjekt tblProjekt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblProjekt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblProjekt);
        }

        // GET: Projekts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblProjekt = await _context.TblProjekt.FindAsync(id);
            if (tblProjekt == null)
            {
                return NotFound();
            }
            return View(tblProjekt);
        }

        // POST: Projekts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjId,ProjName")] TblProjekt tblProjekt)
        {
            if (id != tblProjekt.ProjId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblProjekt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblProjektExists(tblProjekt.ProjId))
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
            return View(tblProjekt);
        }

        // GET: Projekts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblProjekt = await _context.TblProjekt
                .FirstOrDefaultAsync(m => m.ProjId == id);
            if (tblProjekt == null)
            {
                return NotFound();
            }

            return View(tblProjekt);
        }

        // POST: Projekts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblProjekt = await _context.TblProjekt.FindAsync(id);
            _context.TblProjekt.Remove(tblProjekt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblProjektExists(int id)
        {
            return _context.TblProjekt.Any(e => e.ProjId == id);
        }
    }
}
