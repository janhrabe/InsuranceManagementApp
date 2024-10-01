using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceManagementApp.Models
{
    /// <summary>
    /// Model reprezentující Pojistníka
    /// Obsahuje detaily pojistníka, jeho pojištění a pojistných událostí
    /// </summary>
	public class PolicyHolder
	{
        /// <summary>
        /// identické Id pojistníka
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Celé jméno Pojistníka
        /// </summary>
        [Required]
        [Display(Name = "Jméno a Příjmení")]
        public required string FullName { get; set; }
        /// <summary>
        /// Adresa bydliště Pojistníka
        /// </summary>
        [Required]
        [Display(Name = "Adresa")]
        public required string Address { get; set; }
        /// <summary>
        /// Emailová adresa Pojistníka
        /// </summary>
        [Required]
        public required string Email { get; set; }
        /// <summary>
        /// Telefoní číslo pojistníka
        /// </summary>
        [Required]
        [Display(Name = "Telefoní číslo")]
        public required string TelephoneNumber { get; set; }

        /// <summary>
        /// Kolekce Pojištění daného pojistníka
        /// </summary>
        public ICollection<Insurance>? Insurances { get; set; }
        /// <summary>
        /// Kolekce Pojistných událostí daného pojistníka
        /// </summary>
        public ICollection<InsuranceEvent>? InsuranceEvents { get; set; }
    }
}

