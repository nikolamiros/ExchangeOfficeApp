using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeOffice.Models;

namespace ExchangeOffice.DataAccessLayer
{
    public static class ExchangeRepository
    {
        public static List<Valuta> VratiSveValute(bool izuzmiDomacu)
        {
            List<Valuta> result;

            using (var context = new ExchangeDbContext())
            {
                result = context.Valute.Where(it => izuzmiDomacu == false || it.Domaca == false).ToList();
            }

            return result;
        }

        public static List<MenjackaTransakcija> VratiSveMenjackeTransakcije(bool stornirana)
        {
            List<MenjackaTransakcija> transakcije;

            using (var context = new ExchangeDbContext())
            {
                transakcije = context.MenjackeTransakcije.Where(it => it.Stornirana == stornirana).OrderByDescending(it => it.DatumTransakcije).ToList();
            }

            return transakcije;
        }

        public static void StornirajTransackiju(int idTransakcije)
        {
            using (var context = new ExchangeDbContext())
            {
                var transakcija = context.Transakcije.FirstOrDefault(it => it.Id == idTransakcije);
                if (transakcija == null)
                    return;

                transakcija.Stornirana = true;
                context.SaveChanges();
            }
        }

        public static List<RacunBlagajne> VratiSveRacuneBlagajne()
        {
            List<RacunBlagajne> racuni;

            using (var context = new ExchangeDbContext())
            {
                racuni = context.RacuniBlagajne.ToList();
            }

            return racuni;
        }




        public static List<KursnaLista> VratiSveKursneListe()
        {
            List<KursnaLista> kursneListe;

            using (var context = new ExchangeDbContext())
            {
                kursneListe = context.KursneListe.ToList();
            }

            return kursneListe;
        }

        public static List<StavkaKursneListe> VratiStavkeKursneListe(
            string datumListe,
            string valutaListe)
        {
            var result = new List<StavkaKursneListe>();

            if (string.IsNullOrWhiteSpace(datumListe) || string.IsNullOrWhiteSpace(valutaListe))
                return result;

            var date = DateTime.ParseExact(datumListe, "M/dd/yyyy", null);

            using (var context = new ExchangeDbContext())
            {
                result = context.StavkeKursnihLista.Where(it =>
                    it.DatumKursneListe == date &&
                    it.SifraValuteKursneListe.ToLower().Trim() == valutaListe.ToLower().Trim()).ToList();
            }

            return result;
        }

        public static void KreirajDanasnjuKursnuListu()
        {
            var today = DateTime.Today;

            using (var context = new ExchangeDbContext())
            {
                var valute = context.Valute.ToList();

                var domacaValuta = valute.FirstOrDefault(it => it.Domaca);

                if (domacaValuta == null) throw new Exception("Nije postavljena domaca valuta!!!");

                var straneValute = valute.Where(it => it.Sifra != domacaValuta.Sifra).ToList();

                var kursnaLista = new KursnaLista
                {
                    Valuta = domacaValuta,
                    Opis = "Kursna lista na dan " + today.ToShortDateString(),
                    Datum = today,
                    Stavke = straneValute.Select(it => new StavkaKursneListe { ValutaStavke = it }).ToList()
                };

                context.KursneListe.Add(kursnaLista);
                context.SaveChanges();
            }
        }

        internal static void IzmeniStavkuKursneListe(
            string valutaListe,
            string datumListe,
            string valutaStavke,
            decimal kupovniKurs,
            decimal srednjiKurs,
            decimal prodajniKurs)
        {
            var date = DateTime.ParseExact(datumListe, "M/dd/yyyy", null);

            using (var context = new ExchangeDbContext())
            {
                var stavkaZaIzmenu = context.StavkeKursnihLista.FirstOrDefault(
                    it => it.DatumKursneListe == date
                       && it.SifraValuteKursneListe.ToLower().Trim() == valutaListe.ToLower().Trim()
                       && it.SifraValutaStavke.ToLower().Trim() == valutaStavke.ToLower().Trim());

                if (stavkaZaIzmenu != null)
                {
                    stavkaZaIzmenu.ProdajniKurs = prodajniKurs;
                    stavkaZaIzmenu.KupovniKurs = kupovniKurs;
                    stavkaZaIzmenu.SrednjiKurs = srednjiKurs;
                }

                context.SaveChanges();
            }
        }
    }
}