using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InsuranceManagementApp.Models;

namespace InsuranceManagementApp.Controllers;

/// <summary>
/// Řídící třída pro zpracování hlavních stránek aplikace
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Konstruktor pro inicializaci loggeru
    /// </summary>
    /// <param name="logger">Instance loggeru</param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Akce pro zobrazení hlavní stránky
    /// </summary>
    /// <returns>Vrací View pro úvodní stránku</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Akce pro zobrazení stránky s informacemi o ochraně osobních údajů (Privacy Policy)
    /// </summary>
    /// <returns>Vrací View pro stránku ochrany osobních údajů</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Akce pro zobrazení chybové stránky
    /// </summary>
    /// <returns>Vrací View s modelem ErrorViewModel obsahujícím ID žádosti</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

