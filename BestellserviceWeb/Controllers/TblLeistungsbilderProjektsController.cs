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
            for(int i = 0; i < depthList.Count; i++)
            {
                listWithAll.MoneyAmount.Add(0);
                listWithAll.LeistungsbildActive.Add(false);
                listWithAll.LeistungsbildID.Add(leistungsbilderEdited[i].LeistId);
                listWithAll.LeistungsbildName.Add(leistungsbilderEdited[i].LeistName);
            }

            for (int i = 0; i < depthList.Count; i++)
            {
            }

            return View(listWithAll);
        }

        // POST: TblLeistungsbilderProjekts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeistungsbilderProjektList tblLeistungsbilderProjekt)
        {
            List<TblLeistungsbilderProjekt> actualList = new List<TblLeistungsbilderProjekt>();
            
            for(int i = 0; i < tblLeistungsbilderProjekt.LeistungsbildID.Count(); i++)
            {
                if (tblLeistungsbilderProjekt.LeistungsbildActive[i])
                {
                    if (tblLeistungsbilderProjekt.MoneyAmount[i] > 0)
                    {
                        actualList.Add(new TblLeistungsbilderProjekt(tblLeistungsbilderProjekt.ProjektID, tblLeistungsbilderProjekt.LeistungsbildID[i],((double)tblLeistungsbilderProjekt.MoneyAmount[i])));
                    }
                    else
                    {
                        actualList.Add(new TblLeistungsbilderProjekt(tblLeistungsbilderProjekt.ProjektID, tblLeistungsbilderProjekt.LeistungsbildID[i]));
                    }
                }
            }

            for(int i = 0; i < actualList.Count; i++)
            {
                _context.Add(actualList[i]);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditAll(int id)
        {
            List<TblLeistungsbilder> leistungsbilder = _context.TblLeistungsbilder.ToList();
            List<TblLeistungsbilder> leistungsbilderEdited = leistungsbilder.ConvertAll(s => new TblLeistungsbilder { LeistId = s.LeistId, LeistName = s.LeistName, LeistParent = s.LeistParent }).ToList();
            List<int> depthList = new List<int>();
            LeistungsbilderProjektList listWithAll = new LeistungsbilderProjektList();
            List<TblLeistungsbilderProjekt> leistungsbilderProjekt = _context.TblLeistungsbilderProjekt.Where(p => p.LeistpProjekt == id).ToList();


            int depth = 0;
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
            bool found;
            for (int i = 0; i < leistungsbilderEdited.Count; i++)
            {
                found = false;
                for(int j = 0; j < leistungsbilderProjekt.Count; j++)
                {
                    if(leistungsbilderEdited[i].LeistId == leistungsbilderProjekt[j].LeistpLeistungsbild)
                    {
                        if (leistungsbilderProjekt[j].LeistpAmount.HasValue)
                        {
                            listWithAll.MoneyAmount.Add((double)leistungsbilderProjekt[j].LeistpAmount);
                        }
                        else
                        {
                            listWithAll.MoneyAmount.Add(0);
                        }
                        listWithAll.LeistungsbildActive.Add(true);
                        listWithAll.LeistungsbildID.Add(leistungsbilderProjekt[j].LeistpLeistungsbild);
                        listWithAll.LeistungsbildName.Add(leistungsbilderEdited[i].LeistName);
                        found = true;
                    }
                }
                if (!found)
                {
                    listWithAll.MoneyAmount.Add(0);
                    listWithAll.LeistungsbildActive.Add(false);
                    listWithAll.LeistungsbildID.Add(leistungsbilderEdited[i].LeistId);
                    listWithAll.LeistungsbildName.Add(leistungsbilderEdited[i].LeistName);
                }
            }
            listWithAll.ProjektID = id;

            return View(listWithAll);
        }

        // POST: TblLeistungsbilderProjekts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll(LeistungsbilderProjektList tblLeistungsbilderProjekt)
        {
            List<TblLeistungsbilderProjekt> actualList = new List<TblLeistungsbilderProjekt>();
            List<TblLeistungsbilderProjekt> allLeistungsbilderProjekt = _context.TblLeistungsbilderProjekt.Where(p => p.LeistpProjekt == tblLeistungsbilderProjekt.ProjektID).ToList();

            bool found;
            int foundID;
            for (int i = 0; i < tblLeistungsbilderProjekt.LeistungsbildID.Count(); i++)
            {
                if (tblLeistungsbilderProjekt.LeistungsbildActive[i])
                {
                    found = false;
                    foundID = 0;
                    for(int j =0; j < allLeistungsbilderProjekt.Count; j++)
                    {
                        if(allLeistungsbilderProjekt[j].LeistpLeistungsbild == tblLeistungsbilderProjekt.LeistungsbildID[i])
                        {
                            found = true;
                            foundID = allLeistungsbilderProjekt[j].LeistpId;
                        }
                    }
                    if (found)
                    {
                        if (tblLeistungsbilderProjekt.MoneyAmount[i] > 0)
                        {
                            TblLeistungsbilderProjekt temp = new TblLeistungsbilderProjekt(foundID ,tblLeistungsbilderProjekt.ProjektID, tblLeistungsbilderProjekt.LeistungsbildID[i], ((double)tblLeistungsbilderProjekt.MoneyAmount[i]));                          
                            _context.Attach(temp);
                            _context.Entry(temp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                        }
                        else
                        {
                            TblLeistungsbilderProjekt temp = new TblLeistungsbilderProjekt(foundID,tblLeistungsbilderProjekt.ProjektID, tblLeistungsbilderProjekt.LeistungsbildID[i]);
                            _context.Attach(temp);
                            _context.Entry(temp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                        }
                    }
                    else
                    {
                        if (tblLeistungsbilderProjekt.MoneyAmount[i] > 0)
                        {
                            TblLeistungsbilderProjekt temp = new TblLeistungsbilderProjekt(tblLeistungsbilderProjekt.ProjektID, tblLeistungsbilderProjekt.LeistungsbildID[i], ((double)tblLeistungsbilderProjekt.MoneyAmount[i]));                         
                            _context.Attach(temp);
                            _context.Entry(temp).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        }else
                        {
                            TblLeistungsbilderProjekt temp = new TblLeistungsbilderProjekt(tblLeistungsbilderProjekt.ProjektID, tblLeistungsbilderProjekt.LeistungsbildID[i]);
                            _context.Attach(temp);
                            _context.Entry(temp).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
