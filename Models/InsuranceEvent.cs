using System;
using System.ComponentModel.DataAnnotations;
using InsuranceManagementApp.Models;

namespace InsuranceManagementApp.Models
{
	public class InsuranceEvent
	{
        /// <summary>
        /// Identifikace Pojistníka, ke kterému Pojistná Událost patří
        /// </summary>
        public int PolicyHolderId { get; set; }
        [Display(Name = "Pojistník")]
        public PolicyHolder? PolicyHolder { get; set; }
        /// <summary>
        /// Identifikace Pojištění, ke kterému Pojistná Událost patří
        /// </summary>
        public int InsuranceId { get; set; }
        [Display(Name = "Pojištění")]
        public Insurance? Insurance { get; set; }

        /// <summary>
        /// Identické Id Pojistné události
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Datum nahlášení pojistné události
        /// </summary>
        [Display(Name = "Datum")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }
        /// <summary>
        /// Popis Pojistné události
        /// </summary>
        [Display(Name = "Popis události")]
        public required string Description { get; set; }
        /// <summary>
        /// Enum reprezntující stav pojistné události(Nová, schválená,...)
        /// </summary>
        [Display(Name = "Stav")]
        public EventStatus EventStatus { get; set; }

 
	}
}

