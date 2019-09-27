using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeOffice.ViewModels
{
    public class StavkaKursneListeEditViewModel
    {
        public string ValutaListe { get; set; }

        public string DatumListe { get; set; }

        [Display(Name = "Valuta")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public string Valuta { get; set; }

        [Required]
        [Display(Name = "Kupovni kurs")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public decimal KupovniKurs { get; set; }

        [Required]
        [Display(Name = "Prodajni kurs")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public decimal ProdajniKurs { get; set; }

        [Required]
        [Display(Name = "Srednji kurs")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public decimal SrednjiKurs { get; set; }
    }
}