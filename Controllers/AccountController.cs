using InsuranceManagementApp.Controllers;
using InsuranceManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PojistovnaApp.Controllers
{
    /// <summary>
    /// Controller pro správu uživatelských účtů
    /// Tento controller umožňuje uživateli přihlášení, registraci a odhlášení
    /// Používá Identity framework pro správu autentizace
    /// </summary>
    public class AccountController : Controller
    {
        // Základní služby pro správu uživatelů a přihlášení
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        /// <summary>
        /// Konstruktor, který inicializuje potřebné služby pro správu uživatelů
        /// </summary>
        /// <param name="userManager">Správce uživatelů (IdentityUser)</param>
        /// <param name="signInManager">Správce přihlášení uživatelů</param>
        public AccountController
        (
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Přesměruje uživatele na danou URL, pokud je lokální, jinak na hlavní stránku
        /// Používá se po přihlášení nebo registraci
        /// </summary>
        /// <param name="returnUrl">URL, kam má být uživatel přesměrován</param>
        /// <returns>IActionResult přesměruje na správnou stránku</returns>
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Zobrazí přihlašovací formulář
        /// </summary>
        /// <param name="returnUrl">URL, kam má být uživatel přesměrován po přihlášení</param>
        /// <returns>View s přihlašovacím formulářem</returns>
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Zpracovává POST požadavek pro přihlášení uživatele
        /// Validuje vstupy a ověřuje přihlašovací údaje uživatele
        /// </summary>
        /// <param name="model">Model s přihlašovacími údaji</param>
        /// <param name="returnUrl">URL, kam má být uživatel přesměrován po přihlášení</param>
        /// <returns>Vrací stránku s formulářem nebo přesměruje na požadovanou stránku</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result =
                    await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                    return RedirectToLocal(returnUrl);

                ModelState.AddModelError("Login error", "Neplatné přihlašovací údaje.");
                return View(model);
            }
            return View(model);
        }

        /// <summary>
        /// Zobrazí registrační formulář
        /// </summary>
        /// <param name="returnUrl">URL, kam má být uživatel přesměrován po registraci</param>
        /// <returns>View s registračním formulářem</returns>
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Zpracovává POST požadavek pro registraci nového uživatele
        /// Validuje vstupy a vytváří nového uživatele
        /// </summary>
        /// <param name="model">Model s registračními údaji</param>
        /// <param name="returnUrl">URL, kam má být uživatel přesměrován po registraci</param>
        /// <returns>Vrací stránku s formulářem nebo přesměruje na požadovanou stránku</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Odhlásí aktuálního uživatele
        /// </summary>
        /// <returns>Po odhlášení přesměruje na hlavní stránku</returns>
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToLocal(null);
        }
    }
}