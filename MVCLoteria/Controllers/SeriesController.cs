using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatosLoteria.Contexto;
using DatosLoteria.Modelos;

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
            var loteriaContext = _context.Series.Include(s => s.FkSocioNavigation).Include(s => s.FkSorteoNavigation);

            return View(await loteriaContext.ToListAsync());
        }

        // GET: Series/Details/5
        // GET: Series/Details/5 ... /5(sorteo)/1(inicio)
        [Route("series/[action]/{sorteo}/{inicio}")]
        public async Task<IActionResult> Details(int? sorteo, int? inicio)
        {
            if (sorteo == null || inicio == null)
            {
                return NotFound();
            }

            var serie = await _context.Series
                .Include(s => s.FkSocioNavigation)
                .Include(s => s.FkSorteoNavigation)
                .FirstOrDefaultAsync(m => m.FkSorteo == sorteo && m.Inicio == inicio);

            if (serie == null)
            {
                return NotFound();
            }

            return View(serie);
        }

        // GET: Series/Create
        public IActionResult Create()
        {
            ListasSelect();

            return View();
        }

        private void ListasSelect()
        {
            ViewData["FkSocio"] = new SelectList(_context.Socios.OrderBy(s => s.NombreCompleto), "IdSocio", "NombreCompleto");
            ViewData["FkSorteo"] = new SelectList(_context.Sorteos.OrderByDescending(s => s.Fecha), "IdSorteo", "Fecha");
        }

        // POST: Series/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FkSorteo,FkSocio,Inicio,Fin")] Serie serie)
        {
            ValidacionesPropias(serie);
            if (ModelState.IsValid)
            {
                _context.Add(serie);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ListasSelect();

            return View(serie);
        }

        private void ValidacionesPropias(Serie serie)
        {
            if (ValidacionInicioFin(serie))
            {
                ValidacionNoVendida(serie);
            }
        }

        private bool ValidacionNoVendida(Serie serie)
        {
            var serieMadre = _context.Series.Where(s => (s.FkSorteo == serie.FkSorteo) && ((s.Inicio >= serie.Inicio && s.Inicio <= serie.Fin) || (s.Fin >= serie.Inicio && s.Fin <= serie.Fin) || (serie.Inicio >= s.Inicio && serie.Inicio <= s.Fin))).FirstOrDefault();

            if (serieMadre == default) { return true; }

            ModelState.AddModelError("Fin", "Parte de la serie ya está vendida ...");

            return false;
        }

        private bool ValidacionInicioFin(Serie serie)
        {
            if (serie.Inicio <= serie.Fin) { return true; }

            ModelState.AddModelError("Fin", "Fin debe ser mayor o igual que Inicio ...");

            return false;
        }

        // GET: Series/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serie = await _context.Series.FindAsync(id);
            if (serie == null)
            {
                return NotFound();
            }
            ViewData["FkSocio"] = new SelectList(_context.Socios, "IdSocio", "Apellidos", serie.FkSocio);
            ViewData["FkSorteo"] = new SelectList(_context.Sorteos, "IdSorteo", "IdSorteo", serie.FkSorteo);
            return View(serie);
        }

        // POST: Series/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FkSorteo,FkSocio,Inicio,Fin")] Serie serie)
        {
            if (id != serie.FkSorteo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SerieExists(serie.FkSorteo))
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
            ViewData["FkSocio"] = new SelectList(_context.Socios, "IdSocio", "Apellidos", serie.FkSocio);
            ViewData["FkSorteo"] = new SelectList(_context.Sorteos, "IdSorteo", "IdSorteo", serie.FkSorteo);
            return View(serie);
        }

        // GET: Series/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serie = await _context.Series
                .Include(s => s.FkSocioNavigation)
                .Include(s => s.FkSorteoNavigation)
                .FirstOrDefaultAsync(m => m.FkSorteo == id);
            if (serie == null)
            {
                return NotFound();
            }

            return View(serie);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serie = await _context.Series.FindAsync(id);
            if (serie != null)
            {
                _context.Series.Remove(serie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SerieExists(int id)
        {
            return _context.Series.Any(e => e.FkSorteo == id);
        }
    }
}
