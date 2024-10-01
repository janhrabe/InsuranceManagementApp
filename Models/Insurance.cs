using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceManagementApp.Models
{
    /// <summary>
    /// Model pro repreznetaci pojištění
    /// Obsahuje informace o pojistníkovi, typu, částce a období platnosti
    /// </summary>
    public class Insurance
    {
        /// <summary>
        /// Identifikace Pojistníka, kterému Pojištění patří
        /// </summary>
        public int PolicyHolderId { get; set; }
        [Display(Name = "Pojistník")]
        public PolicyHolder? PolicyHolder { get; set; }
        /// <summary>
        /// Identické Id pojištění
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Enum reprezntující typ pojištění (Nemovitosti, Autopojištění,...)
        /// </summary>
        [Display(Name = "Typ pojištění")]
        public InsuranceType InsuranceType { get; set; }
        /// <summary>
        /// Částka určující hodnotu pojištění
        /// </summary>
        [Display(Name = "Částka")]
        public double Amount { get; set; }
        /// <summary>
        /// Datum počátku platnosti
        /// </summary>
        [Display(Name = "Platné od")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Datum konce platnosti
        /// </summary>
        [Display(Name = "Platné do")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Kolekce Pojistných Událostí k danému pojištění
        /// </summary>
        public ICollection<InsuranceEvent>? InsuranceEvents { get; set; }

        
    }
}
