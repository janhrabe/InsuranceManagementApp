using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceManagementApp.Models
{
	/// <summary>
	/// Model pro reprezentaci Kontaktního Formuláře
	/// Obsahuje jméno a email uživatele, který posílá dotaz, samotnou zprávu a datum odeslání
	/// </summary>
	public class ContactForm
	{
		/// <summary>
		/// Id odeslané zprávy
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Jméno a příjmení uživatele
		/// </summary>
		[Display(Name="Jméno a Příjmení")]
		public required string FullName { get; set; }
		/// <summary>
		/// Email uživatele, pro možnost zoětného kontaktu
		/// </summary>
		public required string Email { get; set; }
		/// <summary>
		/// Samotná zpráva zadaná uživatelem
		/// </summary>
		[Display(Name="Vaše zpráva")]
		public required string Message { get; set; }
		/// <summary>
		/// Datum odeslání zprávy/požadavku
		/// </summary>
		[Display(Name = "Datum")]
		[DataType(DataType.Date)]
        public DateTime DateSubmitted { get; set; } = DateTime.Now;
	}
}

