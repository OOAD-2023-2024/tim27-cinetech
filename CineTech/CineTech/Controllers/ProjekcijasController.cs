﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CineTech.Data;
using CineTech.Models;
using Microsoft.AspNetCore.Authorization;

namespace CineTech.Controllers
{
    public class ProjekcijasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjekcijasController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> OdabirSjedista(int? projekcijaId)
        {
            ViewBag.id = projekcijaId;

           /* var projekcija = await _context.Projekcija
                                               .FirstOrDefaultAsync(p => p.id == projekcijaId);

            if (projekcija == null)
            {
                return NotFound();
            }
            var kinosala = _context.KinoSala
                                   .FirstOrDefault(k => k.id == projekcija.kinoSalaId);

            if (kinosala == null)
            {
                return NotFound();
            }

            var sviRedoviMjesta = from red in Enumerable.Range(1, kinosala.brojRedova)
                                  from mjesto in Enumerable.Range(1, kinosala.brojKolona)
                                  select new { Red = red, Mjesto = mjesto };

            var rezervisanaMjesta = _context.ZauzetaSjedista
                                            .Where(r => r.ProjekcijaId == projekcijaId)
                                            .Select(r => new { Red = r.red, Mjesto = r.redniBrojSjedista })
                                            .ToList();

            var slobodnaMjesta = sviRedoviMjesta
                                 .Where(m => !rezervisanaMjesta.Any(rm => rm.Red == m.Red && rm.Mjesto == m.Mjesto))
                                 .ToList();
           */
            return View();
        }

        // POST: ZauzetaSjedistas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Korisnik")]

        public async Task<IActionResult> OdabirSjedista([Bind("red,redniBrojSjedista,ProjekcijaId")] ZauzetaSjedista zauzetaSjedista)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zauzetaSjedista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zauzetaSjedista);
        }

        // GET: Projekcijas
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Index()
        {
            return View(await _context.Projekcija.ToListAsync());
        }
        public async Task<IActionResult> ProjekcijeFilma(int? id) 
        {
            ViewBag.filmid = id;
            if (id == null)
            {
                return NotFound();
            }
            var projekcije = await _context.Projekcija
                .Where(o => o.filmId == id)
                .ToListAsync();

            if (projekcije == null)
            {
                return NotFound();
            }

            return View(projekcije);
        }

        // GET: Projekcijas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekcija = await _context.Projekcija
                .FirstOrDefaultAsync(m => m.id == id);
            if (projekcija == null)
            {
                return NotFound();
            }

            return View(projekcija);
        }

        /*   GET: Projekcijas/Create
          public IActionResult Create()
          {
              var filmoviList = _context.Film.ToList();
              var kinoSaleList = _context.KinoSala.ToList();

              ViewBag.FilmoviList = filmoviList;
              ViewBag.KinoSaleList = kinoSaleList;
              ViewBag.Filmovi = new SelectList(filmoviList, "id", "naziv");
              ViewBag.KinoSale = new SelectList(kinoSaleList, "id", "naziv");

              return View();
          }

          // POST: Projekcijas/Create
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create([Bind("id,datum,vrijeme,cijenaOsnovneKarte,kinoSalaId,filmId")] Projekcija projekcija)
          {

              if (!_context.Film.Any(f => f.id == projekcija.filmId))
              {
                  ModelState.AddModelError("FilmId", "Film with the specified ID does not exist.");
              }

              if (!_context.KinoSala.Any(ks => ks.id == projekcija.kinoSalaId))
              {
                  ModelState.AddModelError("KinoSalaId", "Kino sala with the specified ID does not exist.");
              }

              var filmExists = _context.Film.Any(f => f.id == projekcija.filmId);
              var kinoSalaExists = _context.KinoSala.Any(ks => ks.id == projekcija.kinoSalaId);

              if (!filmExists)
              {
                  ModelState.AddModelError("filmId", "Film ID does not exist.");
              }

              if (!kinoSalaExists)
              {
                  ModelState.AddModelError("kinoSalaId", "Kino Sala ID does not exist.");
              }

              if (ModelState.IsValid)
              {
                  _context.Add(projekcija);
                  await _context.SaveChangesAsync();
                  return RedirectToAction(nameof(Index));
              }

              return View(projekcija);
          }*/
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            var filmoviList = _context.Film.ToList();
            var kinoSaleList = _context.KinoSala.ToList();

            ViewBag.FilmoviList = filmoviList;
            ViewBag.KinoSaleList = kinoSaleList;

            return View();
        }

        // POST: Projekcijas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("id,datum,vrijeme,cijenaOsnovneKarte,kinoSalaId,filmId")] Projekcija projekcija)
        {
            var filmExists = _context.Film.Any(f => f.id == projekcija.filmId);
            var kinoSalaExists = _context.KinoSala.Any(ks => ks.id == projekcija.kinoSalaId);

            if (!filmExists)
            {
                ModelState.AddModelError("filmId", "Film ID does not exist.");
            }

            if (!kinoSalaExists)
            {
                ModelState.AddModelError("kinoSalaId", "Kino Sala ID does not exist.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(projekcija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload the lists if the model state is invalid
            ViewBag.FilmoviList = _context.Film.ToList();
            ViewBag.KinoSaleList = _context.KinoSala.ToList();

            return View(projekcija);
        }


        // GET: Projekcijas/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            var filmoviList = _context.Film.ToList();
            var kinoSaleList = _context.KinoSala.ToList();

            ViewBag.FilmoviList = filmoviList;
            ViewBag.KinoSaleList = kinoSaleList;
            if (id == null)
            {
                return NotFound();
            }

            var projekcija = await _context.Projekcija.FindAsync(id);
            if (projekcija == null)
            {
                return NotFound();
            }
            return View(projekcija);
        }

        // POST: Projekcijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("id,datum,vrijeme,cijenaOsnovneKarte,kinoSalaId,filmId")] Projekcija projekcija)
        {
            if (id != projekcija.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projekcija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjekcijaExists(projekcija.id))
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
            ViewBag.FilmoviList = _context.Film.ToList();
            ViewBag.KinoSaleList = _context.KinoSala.ToList();
            return View(projekcija);
        }

        // GET: Projekcijas/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekcija = await _context.Projekcija.FirstOrDefaultAsync(p => p.id == id);
            if (projekcija == null)
            {
                return NotFound();
            }

            return View(projekcija);
        }

        // POST: Projekcijas/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projekcija = await _context.Projekcija.FindAsync(id);
            if (projekcija != null)
            {
                _context.Projekcija.Remove(projekcija);
            }
            var zauzetaSjedista = await _context.ZauzetaSjedista.Where(o => o.ProjekcijaId == id).ToListAsync();
            if (zauzetaSjedista != null)
            {
                foreach (var sjediste in zauzetaSjedista)
                {
                    _context.ZauzetaSjedista.Remove(sjediste);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjekcijaExists(int id)
        {
            return _context.Projekcija.Any(e => e.id == id);
        }

     
    }
}
