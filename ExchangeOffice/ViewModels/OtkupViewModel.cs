using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ExchangeOffice.Business;
using ExchangeOffice.DataAccessLayer;
using ExchangeOffice.Models;

namespace ExchangeOffice.ViewModels
{
    public class OtkupViewModel : IValidatableObject
    {
        public OtkupViewModel()
        {
            Valute = ExchangeRepository.VratiSveValute(izuzmiDomacu: true);
        }
        
        [Required]
        [Display(Name = "Valuta")]
        public string SifraValuta { get; set; }

        [Required(ErrorMessage = "Iznos za otkup mora biti unet.")]
        [Display(Name = "Iznos")]
        public decimal? Iznos { get; set; }

        [Display(Name = "Rezultat")]
        public decimal? Rezultat { get; set; }

        public List<Valuta> Valute { get; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Iznos.HasValue == false)
            {
                yield return new ValidationResult("Iznos mora biti unet.", new List<string>() { "Iznos" });
            }

            if (!Iznos.HasValue)
                yield break;

            var validacije = ExchangeEngine.Validiraj(
                new OtkupCmd()
                {
                    Iznos = Iznos.Value,
                    SifraValuta = SifraValuta
                });

            foreach (var validacija in validacije)
            {
                foreach (var poruka in validacija.Value)
                {
                    yield return new ValidationResult(poruka, new List<string>() { validacija.Key });
                }
            }
        }
    }
}