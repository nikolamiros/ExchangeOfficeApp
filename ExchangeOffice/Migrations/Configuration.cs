using System.Collections.Generic;
using ExchangeOffice.Models;

namespace ExchangeOffice.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ExchangeOffice.DataAccessLayer.ExchangeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ExchangeOffice.DataAccessLayer.ExchangeDbContext context)
        {
            IList<Valuta> defaultValute = new List<Valuta>
            {
                new Valuta() { Domaca = true, Naziv = "Dinar", Sifra = "RSD" },
                new Valuta() { Domaca = false, Naziv = "Evro", Sifra = "EUR" },
                new Valuta() { Domaca = false, Naziv = "Američki dolar", Sifra = "USD" },
                new Valuta() { Domaca = false, Naziv = "Funta sterlinga", Sifra = "GBP" },
                new Valuta() { Domaca = false, Naziv = "Švajcarski franak", Sifra = "CHF" },
                new Valuta() { Domaca = false, Naziv = "Jen", Sifra = "JPY" },
                new Valuta() { Domaca = false, Naziv = "Juan", Sifra = "CNY" },
                new Valuta() { Domaca = false, Naziv = "Turska lira", Sifra = "TRY" },
                new Valuta() { Domaca = false, Naziv = "Indijska rupija", Sifra = "INR" },
                new Valuta() { Domaca = false, Naziv = "Brazilski real", Sifra = "BRL" },
                new Valuta() { Domaca = false, Naziv = "Argentinski pezos", Sifra = "ARS" },
                new Valuta() { Domaca = false, Naziv = "Meksički pezos", Sifra = "MXN" },
                new Valuta() { Domaca = false, Naziv = "Bolivar Fuerte", Sifra = "VEF" },
                new Valuta() { Domaca = false, Naziv = "Severno korejski von", Sifra = "KPW" },
                new Valuta() { Domaca = false, Naziv = "Kubanski pezos", Sifra = "CUP" },
                new Valuta() { Domaca = false, Naziv = "Kongo franak", Sifra = "CDF" },
                new Valuta() { Domaca = false, Naziv = "Iranski rijal", Sifra = "IRR" }
            };

            context.Valute.AddRange(defaultValute);

            var racuniBlagajne = defaultValute.Select(it => new RacunBlagajne
            {
                Valuta = it,
                Opis = it.Naziv,
                Stanje = 0
            });

            context.RacuniBlagajne.AddRange(racuniBlagajne);

            context.SaveChanges();
        }
    }
}
