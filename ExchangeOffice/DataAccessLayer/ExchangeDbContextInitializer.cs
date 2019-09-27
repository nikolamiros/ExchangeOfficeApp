using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ExchangeOffice.Models;

namespace ExchangeOffice.DataAccessLayer
{
    public class ExchangeDbContextInitializer : CreateDatabaseIfNotExists<ExchangeDbContext>
    {
        protected override void Seed(ExchangeDbContext context)
        {
            IList<Valuta> defaultValute = new List<Valuta>();

            defaultValute.Add(new Valuta() { Domaca = true, Naziv = "Dinar", Sifra = "RSD"});
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Evro", Sifra = "EUR" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Američki dolar", Sifra = "USD" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Funta sterlinga", Sifra = "GBP" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Švajcarski franak", Sifra = "CHF" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Jen", Sifra = "JPY" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Juan", Sifra = "CNY" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Turska lira", Sifra = "TRY" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Indijska rupija", Sifra = "INR" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Brazilski real", Sifra = "BRL" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Argentinski pezos", Sifra = "ARS" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Meksički pezos", Sifra = "MXN" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Bolivar Fuerte", Sifra = "VEF" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Severno korejski von", Sifra = "KPW" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Kubanski pezos", Sifra = "CUP" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Kongo franak", Sifra = "CDF" });
            defaultValute.Add(new Valuta() { Domaca = false, Naziv = "Iranski rijal", Sifra = "IRR" });

            context.Valute.AddRange(defaultValute);

            var racuniBlagajne = defaultValute.Select(it => new RacunBlagajne
            {
                Valuta = it,
                Opis = it.Naziv,
                Stanje = 0
            });

            context.RacuniBlagajne.AddRange(racuniBlagajne);

            base.Seed(context);

            context.SaveChanges();
        }
    }
}