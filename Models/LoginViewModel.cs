using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceManagementApp.Models
{
    /// <summary>
    /// Model pro reprezentaci Přihlášení uživatele
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Vyplnění registrovaného Emailu
        /// </summary>
        [Required(ErrorMessage = "Vyplňte emailovou adresu")]
        [EmailAddress(ErrorMessage = "Nepletná emeilová adresa")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        /// <summary>
        /// Vyplnění registrovaného Hesla
        /// </summary>
        [Required(ErrorMessage = "Vyplňte heslo")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public required string Password { get; set; }

        /// <summary>
        /// Možnost zůstat přihlášen, zapamatování přihlašovasích údajů prohlížečem
        /// </summary>
        [Display(Name = "Pamatuj si mě")]
        public bool RememberMe { get; set; }
        }
    
}

