using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ExchangeOffice.Business;
using ExchangeOffice.DataAccessLayer;
using ExchangeOffice.Models;

namespace ExchangeOffice.ViewModels
{
    public class KonverzijaViewModel : IValidatableObject
    {
        public KonverzijaViewModel()
        {
            Valute = ExchangeRepository.VratiSveValute(izuzmiDomacu: true);
        }

        [Required]
        [Display(Name = "Iz valute")]
        public string SifraValutaIz { get; set; }

        [Required(ErrorMessage = "Iznos za konverziju mora biti unet.")]
        [Display(Name = "Iznos")]
        public decimal? IznosIz { get; set; }

        [Required]
        [Display(Name = "U valutu")]
        public string SifraValutaU { get; set; }

        [Display(Name = "Rezultat")]
        public decimal? Rezultat { get; set; }

        public List<Valuta> Valute { get; }

        public string Naziv { get; set; }

        public string Opis { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SifraValutaIz == SifraValutaU)
            {
                yield return new ValidationResult("Ulazna i izlazna valuta moraju se razlikovati.");
            }

            if (IznosIz.HasValue == false)
            {
                yield return new ValidationResult("Iznos mora biti unet.", new List<string>() { "IznosIz" });
            }

            if (!IznosIz.HasValue)
                yield break;

            var validacije = ExchangeEngine.Validiraj(
                new KonverzijaCmd
                {
                    Iznos = IznosIz.Value,
                    SifraValutaIz = SifraValutaIz,
                    SifraValutaU = SifraValutaU,
                    Opis = Opis,
                    Naziv = Naziv
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