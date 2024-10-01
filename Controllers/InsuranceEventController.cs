using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceManagementApp.Data;
using InsuranceManagementApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace InsuranceManagementApp.Controllers
{
    /// <summary>
    /// Kontroler pro správu pojistných událostí.
    /// Obsahuje akce pro zobrazení, vytváření, úpravu a mazání pojistných událostí.
    /// </summary>
    public class InsuranceEventController : Controller
    {
        private readonly InsuranceDbContext _context;

        /// <summary>
        /// Konstruktor, který inicializuje kontext databáze.
        /// </summary>
        /// <param name="context">Databázový kontext pro přístup k pojistným událostem.</param>
        public InsuranceEventController(InsuranceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Zobrazení seznamu všech pojistných událostí.
        /// </summary>
        /// <returns>Vrací seznam pojistných událostí a související pojistky a pojistníky.</returns>
        public async Task<IActionResult> Index()
        {
            var insuranceDbContext = _context.InsuranceEvents.Include(i => i.Insurance).Include(i => i.PolicyHolder);
            return View(await insuranceDbContext.ToListAsync());
        }

        /// <summary>
        /// Zobrazení detailu konkrétní pojistné události.
        /// </summary>
        /// <param name="id">ID pojistné události.</param>
        /// <returns>Vrací detail pojistné události nebo NotFound, pokud nebyla nalezena.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceEvent = await _context.InsuranceEvents
                .Include(i => i.Insurance)
                .Include(i => i.PolicyHolder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceEvent == null)
            {
                return NotFound();
            }

            return View(insuranceEvent);
        }

        /// <summary>
        /// Vytvoření nové pojistné události. Pouze pro administrátory.
        /// </summary>
        /// <returns>Vrací formulář pro vytvoření nové pojistné události.</returns>
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            //Předání enumu EventStatuses do View
            ViewBag.EventStatuses = Enum.GetValues(typeof(EventStatus)).Cast<EventStatus>();

            //Prázdný list pro pojistky, které se načtou podle zvoleného pojistníka
            ViewBag.Insurances = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");

            // Zobrazení seznamu pojistníků
            ViewBag.PolicyHolders = new SelectList(_context.PolicyHolders, "Id", "FullName");
            return View();
        }

        /// <summary>
        /// Načítání pojistek podle zvoleného pojistníka (použito s AJAX voláním).
        /// </summary>
        /// <param name="policyHolderId">ID pojistníka.</param>
        /// <returns>Vrací seznam pojistek pojistníka ve formátu JSON.</returns>
        [HttpGet]
        public JsonResult GetInsurancesByPolicyHolder(int policyHolderId)
        {
            var insurances = _context.Insurances
                                     .Where(i => i.PolicyHolderId == policyHolderId)
                                     .Select(i => new { i.Id, InsuranceType = i.InsuranceType.ToString() })
                                  .ToList();

            return Json(insurances);
        }

        /// <summary>
        /// Uložení nové pojistné události do databáze.
        /// </summary>
        /// <param name="insuranceEvent">Data nové pojistné události.</param>
        /// <returns>Po úspěšném vytvoření přesměruje na seznam událostí.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PolicyHolderId,InsuranceId,Id,EventDate,Description,EventStatus")] InsuranceEvent insuranceEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insuranceEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.EventStatuses = Enum.GetValues(typeof(EventStatus)).Cast<EventStatus>();

            ViewBag.PolicyHolders = new SelectList(_context.PolicyHolders, "Id", "FullName");
            return View(insuranceEvent);
        }

        /// <summary>
        /// Zobrazení formuláře pro úpravu pojistné události. Pouze pro administrátory.
        /// </summary>
        /// <param name="id">ID pojistné události k úpravě.</param>
        /// <returns>Vrací formulář s daty pojistné události.</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceEvent = await _context.InsuranceEvents.FindAsync(id);
            if (insuranceEvent == null)
            {
                return NotFound();
            }
            ViewBag.EventStatuses = Enum.GetValues(typeof(EventStatus)).Cast<EventStatus>();
            ViewData["InsuranceId"] = new SelectList(_context.Insurances, "Id", "InsuranceType", insuranceEvent.InsuranceId);
            ViewData["PolicyHolderId"] = new SelectList(_context.PolicyHolders, "Id", "FullName", insuranceEvent.PolicyHolderId);
            return View(insuranceEvent);
        }

        /// <summary>
        /// Uložení upravené pojistné události do databáze.
        /// </summary>
        /// <param name="id">ID pojistné události k úpravě.</param>
        /// <param name="insuranceEvent">Data upravené pojistné události.</param>
        /// <returns>Po úspěšné úpravě přesměruje na seznam událostí.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PolicyHolderId,InsuranceId,Id,EventDate,Description,EventStatus")] InsuranceEvent insuranceEvent)
        {
            if (id != insuranceEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuranceEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceEventExists(insuranceEvent.Id))
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
            ViewBag.EventStatuses = Enum.GetValues(typeof(EventStatus)).Cast<EventStatus>();
            ViewData["InsuranceId"] = new SelectList(_context.Insurances, "Id", "InsuranceType", insuranceEvent.InsuranceId);
            ViewData["PolicyHolderId"] = new SelectList(_context.PolicyHolders, "Id", "FullName", insuranceEvent.PolicyHolderId);
            return View(insuranceEvent);
        }

        /// <summary>
        /// Zobrazení potvrzovacího dialogu pro smazání pojistné události. Pouze pro administrátory.
        /// </summary>
        /// <param name="id">ID pojistné události k odstranění.</param>
        /// <returns>Vrací detail události k potvrzení smazání.</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceEvent = await _context.InsuranceEvents
                .Include(i => i.Insurance)
                .Include(i => i.PolicyHolder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceEvent == null)
            {
                return NotFound();
            }

            return View(insuranceEvent);
        }

        /// <summary>
        /// Potvrzení smazání pojistné události z databáze.
        /// </summary>
        /// <param name="id">ID pojistné události k odstranění.</param>
        /// <returns>Přesměruje na seznam událostí po úspěšném smazání.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuranceEvent = await _context.InsuranceEvents.FindAsync(id);
            if (insuranceEvent != null)
            {
                _context.InsuranceEvents.Remove(insuranceEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Kontroluje, zda pojistná událost existuje v databázi.
        /// </summary>
        /// <param name="id">ID pojistné události.</param>
        /// <returns>Vrací true, pokud událost existuje, jinak false.</returns>
        private bool InsuranceEventExists(int id)
        {
            return _context.InsuranceEvents.Any(e => e.Id == id);
        }
    }
}
