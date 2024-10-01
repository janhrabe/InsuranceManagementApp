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
    public class ContactFormController : Controller
    {
        private readonly ContactDbContext _context;

        /// <summary>
        /// Konstruktor pro inicializaci databázového kontextu
        /// </summary>
        /// <param name="context">Databázový kontext</param>
        public ContactFormController(ContactDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Zobrazí seznam všech záznamů (dostupné pouze pro administrátory)
        /// </summary>
        /// <returns>View s seznamem záznamů</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactForms.ToListAsync());
        }

        /// <summary>
        /// Zobrazí podrobnosti o konkrétním záznamu podle ID
        /// </summary>
        /// <param name="id">ID záznamu</param>
        /// <returns>View s detaily záznamu nebo NotFound, pokud není nalezen</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactForm = await _context.ContactForms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactForm == null)
            {
                return NotFound();
            }

            return View(contactForm);
        }

        /// <summary>
        /// Zobrazí formulář pro vytvoření nového záznamu v kontaktním formuláři
        /// </summary>
        /// <returns>View s formulářem pro vytvoření</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Zpracuje odeslání formuláře pro vytvoření nového záznamu
        /// </summary>
        /// <param name="contactForm">Model s údaji z formuláře</param>
        /// <returns>Přesměrování na Index, pokud je úspěšné vytvoření, jinak vrátí formulář s chybami</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Message,DateSubmitted")] ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactForm);
        }

        /// <summary>
        /// Zobrazí formulář pro úpravu existujícího záznamu (dostupné pouze pro administrátory)
        /// </summary>
        /// <param name="id">ID záznamu</param>
        /// <returns>View s formulářem pro úpravu nebo NotFound, pokud není nalezen</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactForm = await _context.ContactForms.FindAsync(id);
            if (contactForm == null)
            {
                return NotFound();
            }
            return View(contactForm);
        }

        /// <summary>
        /// Zpracuje odeslání formuláře pro úpravu existujícího záznamu
        /// </summary>
        /// <param name="id">ID záznamu</param>
        /// <param name="contactForm">Model s upravenými údaji</param>
        /// <returns>Přesměrování na Index, pokud je úprava úspěšná, jinak vrátí formulář s chybami</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Message,DateSubmitted")] ContactForm contactForm)
        {
            if (id != contactForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactFormExists(contactForm.Id))
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
            return View(contactForm);
        }

        /// <summary>
        /// Zobrazí potvrzení o odstranění záznamu (dostupné pouze pro administrátory)
        /// </summary>
        /// <param name="id">ID záznamu</param>
        /// <returns>View s potvrzením nebo NotFound, pokud není nalezen</returns>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactForm = await _context.ContactForms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactForm == null)
            {
                return NotFound();
            }

            return View(contactForm);
        }

        /// <summary>
        /// Zpracuje potvrzení o odstranění záznamu
        /// </summary>
        /// <param name="id">ID záznamu</param>
        /// <returns>Přesměrování na Index po úspěšném odstranění</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactForm = await _context.ContactForms.FindAsync(id);
            if (contactForm != null)
            {
                _context.ContactForms.Remove(contactForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Ověří, zda existuje záznam s daným ID
        /// </summary>
        /// <param name="id">ID záznamu</param>
        /// <returns>Vrací true, pokud záznam existuje, jinak false</returns>
        private bool ContactFormExists(int id)
        {
            return _context.ContactForms.Any(e => e.Id == id);
        }
    }
}
