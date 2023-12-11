using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatosLoteria.Data;
using DatosLoteria.Modelos;
using DatosLoteria.Static;

namespace MVCLoteria.Controllers
{
    public class SeriesController : Controller
    {
        private readonly LoteriaContext _context;

        public SeriesController(LoteriaContext context)
        {
            _context = context;
        }

        // GET: Series
        public async Task<IActionResult> Index()
        {
            var loteriaContext = _context.Series;

            return View(await loteriaContext.ToListAsync());
        }

        // GET: Series/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var series = await _context.Series
                .Include(s => s.FkSocioNavigation)
                .Include(s => s.FkSorteoNavigation)
                .FirstOrDefaultAsync(m => m.FkSorteo == id);
            if (series == null)
            {
                return NotFound();
            }

            return View(series);
        }

        // GET: Series/Create
        public IActionResult Create()
        {
            SelectListas();

            return View();
        }

        // POST: Series/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FkSorteo,FkSocio,Inicio,Fin")] Series series)
        {
            ValidacionesPropias(series);
            if (ModelState.IsValid)
            {
                _context.Add(series);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            SelectListas();

            return View(series);
        }

        // GET: Series/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var series = await _context.Series.FindAsync(id);
            if (series == null)
            {
                return NotFound();
            }
            ViewData["FkSocio"] = new SelectList(_context.Socios, "IdSocio", "Apellidos", series.FkSocio);
            ViewData["FkSorteo"] = new SelectList(_context.Sorteos, "IdSorteo", "Numero", series.FkSorteo);
            return View(series);
        }

        // POST: Series/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FkSorteo,FkSocio,Inicio,Fin")] Series series)
        {
            if (id != series.FkSorteo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(series);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeriesExists(series.FkSorteo))
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
            ViewData["FkSocio"] = new SelectList(_context.Socios, "IdSocio", "Apellidos", series.FkSocio);
            ViewData["FkSorteo"] = new SelectList(_context.Sorteos, "IdSorteo", "Numero", series.FkSorteo);
            return View(series);
        }

        // GET: Series/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var series = await _context.Series
                .Include(s => s.FkSocioNavigation)
                .Include(s => s.FkSorteoNavigation)
                .FirstOrDefaultAsync(m => m.FkSorteo == id);
            if (series == null)
            {
                return NotFound();
            }

            return View(series);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var series = await _context.Series.FindAsync(id);
            if (series != null)
            {
                _context.Series.Remove(series);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeriesExists(int id)
        {
            return _context.Series.Any(e => e.FkSorteo == id);
        }

        private void SelectListas()
        {
            ViewData["FkSocio"] = new SelectList(_context.Socios.OrderBy(s => s.NombreCompleto), "IdSocio", "NombreCompleto");
            ViewData["FkSorteo"] = new SelectList(_context.Sorteos.OrderByDescending(s => s.Fecha), "IdSorteo", "Fecha");
        }
        private void ValidacionesPropias(Series series)
        {
            if(ValidarInicioFin(series))
            {
                ValidarSinVender(series);
            }
        }

        private bool ValidarInicioFin(Series series)
        {
            if (series.Inicio <= series.Fin)  { return true; }

            ModelState.AddModelError("Fin", "Fin debe ser mayor o igual que Inicio ...");

            return false;
        }

        private bool ValidarSinVender(Series series)
        {
            var listaSeries = _context.Series.Where(s => (s.FkSorteo == series.FkSorteo) &&
            ((s.Inicio >= series.Inicio && s.Inicio <= series.Fin) ||
            (s.Fin >= series.Inicio && s.Fin <= series.Fin) ||
            (series.Inicio >= s.Inicio && series.Inicio <= s.Fin))).FirstOrDefault();

            if (listaSeries == default) { return true; }

            ModelState.AddModelError("Fin", "Parte de la serie ya está vendida ...");

            return false;
        }
    }
}
