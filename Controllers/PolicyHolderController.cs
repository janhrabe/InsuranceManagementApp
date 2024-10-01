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
    /// Kontroler pro správu pojistníků. Obsahuje akce pro zobrazení, vytváření, úpravu a mazání pojistníků.
    /// </summary>
    public class PolicyHolderController : Controller
    {
        private readonly InsuranceDbContext _context;

        /// <summary>
        /// Konstruktor inicializující databázový kontext pro práci s pojistníky.
        /// </summary>
        /// <param name="context">Databázový kontext.</param>
        public PolicyHolderController(InsuranceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Zobrazení seznamu všech pojistníků.
        /// </summary>
        /// <returns>Vrací seznam pojistníků.</returns>

        public async Task<IActionResult> Index()
        {
            return View(await _context.PolicyHolders.ToListAsync());
        }

        /// <summary>
        /// Zobrazení detailu konkrétního pojistníka.
        /// </summary>
        /// <param name="id">ID pojistníka.</param>
        /// <returns>Vrací detail pojistníka včetně souvisejících pojistek, nebo NotFound, pokud nebyl nalezen.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyHolder = await _context.PolicyHolders
                .Include(p => p.Insurances) //načtení pojistek pojistníka
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policyHolder == null)
            {
                return NotFound();
            }

            return View(policyHolder);
        }

        /// <summary>
        /// Zobrazení formuláře pro vytvoření nového pojistníka. Pouze pro administrátory.
        /// </summary>
        /// <returns>Vrací formulář pro vytvoření nového pojistníka.</returns>
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Uložení nového pojistníka do databáze.
        /// </summary>
        /// <param name="policyHolder">Data nového pojistníka.</param>
        /// <returns>Po úspěšném vytvoření přesměruje na seznam pojistníků.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Address,Email,TelephoneNumber")] PolicyHolder policyHolder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(policyHolder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(policyHolder);
        }

        /// <summary>
        /// Zobrazení formuláře pro úpravu existujícího pojistníka. Pouze pro administrátory.
        /// </summary>
        /// <param name="id">ID pojistníka k úpravě.</param>
        /// <returns>Vrací formulář s daty pojistníka k úpravě, nebo NotFound, pokud nebyl nalezen.</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyHolder = await _context.PolicyHolders.FindAsync(id);
            if (policyHolder == null)
            {
                return NotFound();
            }
            return View(policyHolder);
        }

        /// <summary>
        /// Uložení upraveného pojistníka do databáze.
        /// </summary>
        /// <param name="id">ID pojistníka k úpravě.</param>
        /// <param name="policyHolder">Data upraveného pojistníka.</param>
        /// <returns>Po úspěšné úpravě přesměruje na seznam pojistníků.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Address,Email,TelephoneNumber")] PolicyHolder policyHolder)
        {
            if (id != policyHolder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policyHolder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyHolderExists(policyHolder.Id))
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
            return View(policyHolder);
        }

        /// <summary>
        /// Zobrazení potvrzovacího dialogu pro smazání pojistníka. Pouze pro administrátory.
        /// </summary>
        /// <param name="id">ID pojistníka k odstranění.</param>
        /// <returns>Vrací detail pojistníka pro potvrzení smazání, nebo NotFound, pokud nebyl nalezen.</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyHolder = await _context.PolicyHolders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policyHolder == null)
            {
                return NotFound();
            }

            return View(policyHolder);
        }

        /// <summary>
        /// Potvrzení smazání pojistníka z databáze.
        /// </summary>
        /// <param name="id">ID pojistníka k odstranění.</param>
        /// <returns>Přesměruje na seznam pojistníků po úspěšném smazání.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policyHolder = await _context.PolicyHolders.FindAsync(id);
            if (policyHolder != null)
            {
                _context.PolicyHolders.Remove(policyHolder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Kontroluje, zda pojistník existuje v databázi.
        /// </summary>
        /// <param name="id">ID pojistníka.</param>
        /// <returns>Vrací true, pokud pojistník existuje, jinak false.</returns>
        private bool PolicyHolderExists(int id)
        {
            return _context.PolicyHolders.Any(e => e.Id == id);
        }
    }
}
