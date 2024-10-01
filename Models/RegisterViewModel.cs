using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceManagementApp.Models
{
    /// <summary>
    /// Model reprezntujíci Registraci uživatele
    /// </summary>
	public class RegisterViewModel
	{
        /// <summary>
        /// Zadání jedinečného Emailu
        /// </summary>
        [Required(ErrorMessage = "Vyplňtě emailovou adresu")]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        /// <summary>
        /// Zadání jedinečného Hesla
        /// </summary>
        [Required(ErrorMessage = "Vyplňtě heslo")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = "";

        /// <summary>
        /// Potvrzení jedinečného Hesla
        /// </summary>
        [Required(ErrorMessage = "Vyplňte heslo.")]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení hesla.")]
        [Compare(nameof(Password), ErrorMessage = "Zadaná hesla se musí shodovat.")]
        public string ConfirmPassword { get; set; } = "";
    }
}

