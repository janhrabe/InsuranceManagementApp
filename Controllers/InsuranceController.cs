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
    /// Kontroler pro správu pojistění.
    /// Obsahuje akce pro vytváření, úpravy, mazání a zobrazení pojistění.
    /// </summary>
    public class InsuranceController : Controller
    {
        private readonly InsuranceDbContext _context;

        /// <summary>
        /// Konstruktor kontroleru, který inicializuje kontext databáze.
        /// </summary>
        /// <param name="context">Databázový kontext pro přístup k datům o pojištěních.</param>
        public InsuranceController(InsuranceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Akce pro zobrazení seznamu všech pojištění.
        /// </summary>
        /// <returns>Vrací View s listem všech pojištění, jejich držitelů a dalších detailů.</returns>
        public async Task<IActionResult> Index()
        {
            var insuranceDbContext = _context.Insurances.Include(i => i.PolicyHolder);
            return View(await insuranceDbContext.ToListAsync());
        }

        /// <summary>
        /// Akce pro zobrazení detailů konkrétní pojistky.
        /// </summary>
        /// <param name="id">ID pojištění.</param>
        /// <returns>Vrací detailní zobrazení konkrétní pojistky.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .Include(i => i.PolicyHolder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        /// <summary>
        /// Akce pro vytvoření nové pojistky. Pouze pro administrátory.
        /// </summary>
        /// <returns>Vrací formulář pro vytvoření pojištění.</returns>
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewBag.InsuranceTypes = Enum.GetValues(typeof(InsuranceType)).Cast<InsuranceType>();

            ViewData["PolicyHolderId"] = new SelectList(_context.PolicyHolders, "Id", "FullName");
            return View();
        }

        /// <summary>
        /// Akce pro uložení nově vytvořené pojistky do databáze.
        /// </summary>
        /// <param name="insurance">Instance pojištění obsahující data z formuláře.</param>
        /// <returns>Po úspěšném vytvoření přesměruje na seznam pojistek, jinak znovu zobrazí formulář.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PolicyHolderId,Id,InsuranceType,Amount,StartDate,EndDate")] Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.InsuranceTypes = Enum.GetValues(typeof(InsuranceType)).Cast<InsuranceType>();
            ViewData["PolicyHolderId"] = new SelectList(_context.PolicyHolders, "Id", "FullName", insurance.PolicyHolderId);
            return View(insurance);
        }

        /// <summary>
        /// Akce pro úpravu existujícího pojištění. Pouze pro administrátory.
        /// </summary>
        /// <param name="id">ID pojistky k úpravě.</param>
        /// <returns>Vrací formulář pro úpravu pojištění.</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }
            ViewData["PolicyHolderId"] = new SelectList(_context.PolicyHolders, "Id", "FullName", insurance.PolicyHolderId);
            return View(insurance);
        }

        /// <summary>
        /// Akce pro uložení úprav existujícího pojištění.
        /// </summary>
        /// <param name="id">ID pojistky k úpravě.</param>
        /// <param name="insurance">Upravená instance pojištění.</param>
        /// <returns>Po úspěšné úpravě přesměruje na seznam pojistek, jinak znovu zobrazí formulář.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PolicyHolderId,Id,InsuranceType,Amount,StartDate,EndDate")] Insurance insurance)
        {
            if (id != insurance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.Id))
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
            ViewData["PolicyHolderId"] = new SelectList(_context.PolicyHolders, "Id", "FullName", insurance.PolicyHolderId);
            return View(insurance);
        }

        /// <summary>
        /// Akce pro smazání existující pojistky. Pouze pro administrátory.
        /// </summary>
        /// <param name="id">ID pojistky k odstranění.</param>
        
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .Include(i => i.PolicyHolder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        /// <summary>
        /// Potvrzení smazání pojištění z databáze.
        /// </summary>
        /// <param name="id">ID pojištění k odstranění.</param>
        /// <returns>Přesměruje na seznam pojistek po úspěšném odstranění.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance != null)
            {
                _context.Insurances.Remove(insurance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Kontroluje, zda pojistka existuje v databázi.
        /// </summary>
        /// <param name="id">ID pojištění.</param>
        /// <returns>Vrací true, pokud pojistka existuje, jinak false.</returns>
        private bool InsuranceExists(int id)
        {
            return _context.Insurances.Any(e => e.Id == id);
        }
    }
}
