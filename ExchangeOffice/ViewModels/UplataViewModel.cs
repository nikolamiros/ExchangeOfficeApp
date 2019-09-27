using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExchangeOffice.Business;

namespace ExchangeOffice.ViewModels
{
    public class UplataViewModel : IValidatableObject
    {   
        [Required]
        [Display(Name = "Valuta")]
        public string SifraValuta { get; set; }

        [Required(ErrorMessage = "Iznos za uplatu mora biti unet.")]
        [Display(Name = "Iznos")]
        public decimal? Iznos { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validacije = ExchangeEngine.Validiraj(
                new UplataCmd()
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