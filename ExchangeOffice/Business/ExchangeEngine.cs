using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeOffice.DataAccessLayer;
using ExchangeOffice.Models;

namespace ExchangeOffice.Business
{
    public static class ExchangeEngine
    {
        public static Dictionary<string, List<string>> Validiraj(OtkupCmd cmd)
        {
            return IzvrsiInternal(cmd, true);
        }

        public static void Izvrsi(OtkupCmd cmd)
        {
            IzvrsiInternal(cmd);
        }

        public static Dictionary<string, List<string>> IzvrsiInternal(OtkupCmd cmd, bool validacija = false)
        {
            var rezultat = new Dictionary<string, List<string>>();

            if (cmd.Iznos < 0)
            {
                DodajValidacionuGresku(rezultat, "Iznos", "Iznos mora biti veći od 0!");
                if (validacija == false) throw new Exception("Iznos mora biti veći od 0!");
            }

            if (string.IsNullOrEmpty(cmd.SifraValuta))
            {
                DodajValidacionuGresku(rezultat, "SifraValuta", "Valuta mora biti uneta!!!");
                if (validacija == false) throw new Exception("Valuta mora biti uneta!!!");
            }

            if (VratiValutu(cmd.SifraValuta) == null)
            {
                DodajValidacionuGresku(rezultat, "SifraValuta", "Nepostojeca valuta!!!");
                if (validacija == false) throw new Exception("Nepostojeca valuta!!!");
            }

            var sifraDomaceValute = VratiDomacuValutu();

            var stavka = VratiStavkuKursneListe(sifraDomaceValute, cmd.SifraValuta);

            if (stavka == null)
            {
                DodajValidacionuGresku(rezultat, "", "Nepostojeca kursna lista!!!");
                if (validacija == false) throw new Exception("Nepostojeca kursna lista!!!");
            }
            else
            {
                var iznosOtkupa = cmd.Iznos;

                var iznosProdaje = stavka.KupovniKurs * cmd.Iznos;

                var sifraValutaOtkupa = cmd.SifraValuta;

                var sifraValuteProdaje = sifraDomaceValute;

                var menjackaTransakcija = new MenjackaTransakcija
                {
                    DatumTransakcije = DateTime.Now,
                    IznosOtkupa = iznosOtkupa,
                    SifraValutaOtkupa = sifraValutaOtkupa,
                    SifraValutaProdaje = sifraValuteProdaje,
                    Naziv = cmd.Naziv,
                    Opis = cmd.Opis,
                    Tip = TipMenjackeTransakcije.Otkup,
                    IznosProdaje = iznosProdaje,
                    FinansijskeTransakcije =
                        NapraviFinansijskeTransakcije(sifraValutaOtkupa, iznosOtkupa, sifraDomaceValute, iznosProdaje)
                };

                IzvrsiTransakciju(menjackaTransakcija, validacija, rezultat);
            }

            return rezultat;
        }

        public static decimal Izracunaj(OtkupCmd cmd)
        {
            var rezultat = (decimal) 0;

            if (cmd.Iznos < 0)
            {
                return rezultat;
            }

            if (string.IsNullOrEmpty(cmd.SifraValuta))
            {
                return rezultat;
            }

            if (VratiValutu(cmd.SifraValuta) == null)
            {
                return rezultat;
            }

            var sifraDomaceValute = VratiDomacuValutu();

            var stavka = VratiStavkuKursneListe(sifraDomaceValute, cmd.SifraValuta);

            if (stavka == null)
            {
                return rezultat;
            }

            rezultat = stavka.KupovniKurs * cmd.Iznos;

            return rezultat;
        }




        public static Dictionary<string, List<string>> Validiraj(ProdajaCmd cmd)
        {
            return IzvrsiInternal(cmd, true);
        }

        public static void Izvrsi(ProdajaCmd cmd)
        {
            IzvrsiInternal(cmd);
        }

        private static Dictionary<string, List<string>> IzvrsiInternal(ProdajaCmd cmd, bool validacija = false)
        {
            var rezultat = new Dictionary<string, List<string>>();

            if (cmd.Iznos < 0)
            {
                DodajValidacionuGresku(rezultat, "Iznos", "Iznos mora biti veći od 0!");
                if (validacija == false) throw new Exception("Iznos mora biti veći od 0!");
            }

            if (string.IsNullOrEmpty(cmd.SifraValuta))
            {
                DodajValidacionuGresku(rezultat, "SifraValuta", "Valuta mora biti uneta!!!");
                if (validacija == false) throw new Exception("Valuta mora biti uneta!!!");
            }

            if (VratiValutu(cmd.SifraValuta) == null)
            {
                DodajValidacionuGresku(rezultat, "SifraValuta", "Nepostojeca valuta!!!");
                if (validacija == false) throw new Exception("Nepostojeca valuta!!!");
            }

            var sifraDomaceValute = VratiDomacuValutu();

            var stavka = VratiStavkuKursneListe(sifraDomaceValute, cmd.SifraValuta);

            if (stavka == null)
            {
                DodajValidacionuGresku(rezultat, "", "Nepostojeca kursna lista!!!");
                if (validacija == false) throw new Exception("Nepostojeca kursna lista!!!");
            }
            else
            {
                var iznosProdaje = cmd.Iznos;

                var iznosOtkupa = stavka.ProdajniKurs * cmd.Iznos;

                var sifraValuteProdaje = cmd.SifraValuta;

                var sifraValutaOtkupa = sifraDomaceValute;

                var menjackaTransakcija = new MenjackaTransakcija
                {
                    DatumTransakcije = DateTime.Now,
                    IznosOtkupa = iznosOtkupa,
                    SifraValutaOtkupa = sifraValutaOtkupa,
                    SifraValutaProdaje = sifraValuteProdaje,
                    Naziv = cmd.Naziv,
                    Opis = cmd.Opis,
                    Tip = TipMenjackeTransakcije.Prodaja,
                    IznosProdaje = iznosProdaje,
                    FinansijskeTransakcije =
                        NapraviFinansijskeTransakcije(sifraDomaceValute, iznosOtkupa, sifraValuteProdaje, iznosProdaje)
                };

                IzvrsiTransakciju(menjackaTransakcija, validacija, rezultat);
            }

            return rezultat;
        }

        public static decimal Izracunaj(ProdajaCmd cmd)
        {
            var rezultat = (decimal) 0;

            if (cmd.Iznos < 0)
            {
                return rezultat;
            }

            if (string.IsNullOrEmpty(cmd.SifraValuta))
            {
                return rezultat;
            }

            if (VratiValutu(cmd.SifraValuta) == null)
            {
                return rezultat;
            }

            var sifraDomaceValute = VratiDomacuValutu();

            var stavka = VratiStavkuKursneListe(sifraDomaceValute, cmd.SifraValuta);

            if (stavka == null)
            {
                return rezultat;
            }
            
            rezultat = stavka.ProdajniKurs * cmd.Iznos;

            return rezultat;
        }




        public static Dictionary<string, List<string>> Validiraj(KonverzijaCmd cmd)
        {
            return IzvrsiInternal(cmd, true);
        }

        public static void Izvrsi(KonverzijaCmd cmd)
        {
            IzvrsiInternal(cmd);
        }

        private static Dictionary<string, List<string>> IzvrsiInternal(KonverzijaCmd cmd, bool validacija = false)
        {
            var rezultat = new Dictionary<string, List<string>>();

            if (cmd.Iznos < 0)
            {
                DodajValidacionuGresku(rezultat, "Iznos", "Iznos mora biti veći od 0!");
                if (validacija == false) throw new Exception("Iznos mora biti veći od 0!");
            }

            if (string.IsNullOrEmpty(cmd.SifraValutaIz) || string.IsNullOrEmpty(cmd.SifraValutaU))
            {
                DodajValidacionuGresku(rezultat, "SifraValuta", "Valuta mora biti uneta!!!");
                if (validacija == false) throw new Exception("Valuta mora biti uneta!!!");
            }

            if (VratiValutu(cmd.SifraValutaIz) == null)
            {
                DodajValidacionuGresku(rezultat, "SifraValutaIz", $"Nepostojeca valuta {cmd.SifraValutaIz}!!!");
                if (validacija == false) throw new Exception($"Nepostojeca valuta {cmd.SifraValutaIz}!!!");
            }

            if (VratiValutu(cmd.SifraValutaU) == null)
            {
                DodajValidacionuGresku(rezultat, "SifraValutaU", $"Nepostojeca valuta {cmd.SifraValutaU}!!!");
                if (validacija == false) throw new Exception($"Nepostojeca valuta {cmd.SifraValutaU}!!!");
            }

            var sifraDomaceValute = VratiDomacuValutu();

            var stavkaU = VratiStavkuKursneListe(sifraDomaceValute, cmd.SifraValutaU);
            var stavkaIz = VratiStavkuKursneListe(sifraDomaceValute, cmd.SifraValutaIz);

            if (stavkaU == null || stavkaIz == null)
            {
                DodajValidacionuGresku(rezultat, "", "Nepostojeca kursna lista!!!");
                if (validacija == false) throw new Exception("Nepostojeca kursna lista!!!");
            }
            else
            {
                var medjuIznos = stavkaU.KupovniKurs * cmd.Iznos;

                var iznosProdaje = Math.Round(medjuIznos / stavkaIz.ProdajniKurs, 2);

                var sifraValuteProdaje = cmd.SifraValutaU;

                var iznosOtkupa = cmd.Iznos;

                var sifraValutaOtkupa = cmd.SifraValutaIz;

                var menjackaTransakcija = new MenjackaTransakcija
                {
                    DatumTransakcije = DateTime.Now,
                    IznosOtkupa = iznosOtkupa,
                    SifraValutaOtkupa = sifraValutaOtkupa,
                    SifraValutaProdaje = sifraValuteProdaje,
                    Naziv = cmd.Naziv,
                    Opis = cmd.Opis,
                    Tip = TipMenjackeTransakcije.Konverzija,
                    IznosProdaje = iznosProdaje,
                    FinansijskeTransakcije = NapraviFinansijskeTransakcije(sifraValutaOtkupa, iznosOtkupa,
                        sifraDomaceValute, iznosProdaje, medjuIznos)
                };

                IzvrsiTransakciju(menjackaTransakcija, validacija, rezultat);
            }

            return rezultat;
        }

        public static decimal Izracunaj(KonverzijaCmd cmd)
        {
            var rezultat = (decimal)0;

            if (cmd.Iznos < 0 ||
                string.IsNullOrEmpty(cmd.SifraValutaIz) || 
                string.IsNullOrEmpty(cmd.SifraValutaU) || 
                VratiValutu(cmd.SifraValutaIz) == null || 
                VratiValutu(cmd.SifraValutaU) == null ||
                string.Equals(cmd.SifraValutaIz, cmd.SifraValutaU, StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(cmd.SifraValutaIz) == false &&
                    string.IsNullOrEmpty(cmd.SifraValutaU) == false &&
                    string.Equals(cmd.SifraValutaIz, cmd.SifraValutaU, StringComparison.InvariantCultureIgnoreCase))
                    rezultat = cmd.Iznos;

                return rezultat;
            }

            var sifraDomaceValute = VratiDomacuValutu();

            var stavkaU = VratiStavkuKursneListe(sifraDomaceValute, cmd.SifraValutaU);

            var stavkaIz = VratiStavkuKursneListe(sifraDomaceValute, cmd.SifraValutaIz);

            if (stavkaU == null || stavkaIz == null)
            {
                return rezultat;
            }

            var medjuIznos = stavkaIz.KupovniKurs * cmd.Iznos;

            rezultat = Math.Round(medjuIznos / stavkaU.ProdajniKurs, 2);

            return rezultat;
        }



        public static Dictionary<string, List<string>> Validiraj(UplataCmd cmd)
        {
            return IzvrsiInternal(cmd, true);
        }

        public static void Izvrsi(UplataCmd cmd)
        {
            IzvrsiInternal(cmd);
        }

        private static Dictionary<string, List<string>> IzvrsiInternal(UplataCmd cmd, bool validacija = false)
        {
            var rezultat = new Dictionary<string, List<string>>();

            if (cmd.Iznos < 0)
            {
                DodajValidacionuGresku(rezultat, "Iznos", "Iznos mora biti veći od 0!");
                if (validacija == false) throw new Exception("Iznos mora biti veći od 0!");
            }

            if (string.IsNullOrEmpty(cmd.SifraValuta))
            {
                DodajValidacionuGresku(rezultat, "SifraValuta", "Valuta mora biti uneta!!!");
                if (validacija == false) throw new Exception("Valuta mora biti uneta!!!");
            }

            if (VratiValutu(cmd.SifraValuta) == null)
            {
                DodajValidacionuGresku(rezultat, "SifraValuta", "Nepostojeca valuta!!!");
                if (validacija == false) throw new Exception("Nepostojeca valuta!!!");
            }
            else
            {
                var iznos = cmd.Iznos;
                var valuta = cmd.SifraValuta;

                var korekcijaStanjaBlagajne = new KorekcijaStanjaBlagajne()
                {
                    DatumTransakcije = DateTime.Now,
                    Naziv = cmd.Naziv,
                    Opis = cmd.Opis,
                    Iznos = iznos,
                    SifraValute = valuta,
                    FinansijskeTransakcije = new List<FinansijskaTransakcija>
                    {
                        new FinansijskaTransakcija
                        {
                            Iznos = iznos,
                            SifraValuteRacunaBlagajne = valuta,
                            Redosled = 0,
                            Smer = SmerFinansijskeTransakcije.Ulaz
                        }
                    }
                };

                IzvrsiTransakciju(korekcijaStanjaBlagajne, validacija, rezultat);
            }

            return rezultat;
        }



        public static Dictionary<string, List<string>> Validiraj(StornoCmd cmd)
        {
            return IzvrsiInternal(cmd);
        }

        public static void Izvrsi(StornoCmd cmd)
        {
            IzvrsiInternal(cmd);
        }

        private static Dictionary<string, List<string>> IzvrsiInternal(StornoCmd cmd, bool validacija = false)
        {
            var rezultat = new Dictionary<string, List<string>>();

            using (var context = new ExchangeDbContext())
            {
                var transakcija = context.Transakcije.FirstOrDefault(it => it.Id == cmd.IdTransakcije);
                if (transakcija == null)
                {
                    DodajValidacionuGresku(rezultat, "", $"Transakcija sa Id-jem {cmd.IdTransakcije} ne postoji!!!");
                    if (validacija == false) throw new Exception($"Transakcija sa Id-jem {cmd.IdTransakcije} ne postoji!!!");
                }
                else
                {
                    transakcija.Stornirana = true;

                    if (cmd.Opis != null)
                        transakcija.Opis = cmd.Opis;

                    var brojac = transakcija.FinansijskeTransakcije.Max(it => it.Redosled);

                    var stornoFinansijskeTransakcije = transakcija.FinansijskeTransakcije.Select(
                        it => new FinansijskaTransakcija
                        {
                            Redosled = ++brojac,
                            Iznos = it.Iznos,
                            Smer = (it.Smer == SmerFinansijskeTransakcije.Ulaz) ? SmerFinansijskeTransakcije.Izlaz : SmerFinansijskeTransakcije.Ulaz,
                            SifraValuteRacunaBlagajne = it.SifraValuteRacunaBlagajne
                        }).ToList();

                    foreach (var finansijskaTransakcija in stornoFinansijskeTransakcije)
                    {
                        var racun = context.RacuniBlagajne.FirstOrDefault(it =>
                            it.SifraValute.ToLower().Trim() ==
                            finansijskaTransakcija.SifraValuteRacunaBlagajne.ToLower().Trim());

                        if (racun == null)
                        {
                            DodajValidacionuGresku(rezultat, "", $"Ne postoji racun za valutu {finansijskaTransakcija.SifraValuteRacunaBlagajne}!!!");
                            if (validacija == false) throw new Exception($"Ne postoji racun za valutu {finansijskaTransakcija.SifraValuteRacunaBlagajne}!!!");
                        }
                        else
                        {
                            if (racun.Stanje < finansijskaTransakcija.Iznos)
                            {
                                DodajValidacionuGresku(rezultat, "", $"Nema dovoljno sredstava na racunu {racun.SifraValute}!!!");
                                if (validacija == false) throw new Exception($"Nema dovoljno sredstava na racunu {racun.SifraValute}!!!");
                            }
                            else
                            {
                                switch (finansijskaTransakcija.Smer)
                                {
                                    case SmerFinansijskeTransakcije.Izlaz:
                                        racun.Stanje -= finansijskaTransakcija.Iznos;
                                        break;
                                    default:
                                        racun.Stanje += finansijskaTransakcija.Iznos;
                                        break;
                                }
                                transakcija.FinansijskeTransakcije.Add(finansijskaTransakcija);
                            }
                        }
                    }

                    if (validacija == false)
                        context.SaveChanges();
                }
            }

            return rezultat;
        }



        private static void IzvrsiTransakciju(
            BaznaTransakcija transakcija,
            bool validacija,
            IDictionary<string, List<string>> validacioneGreske)
        {
            using (var context = new ExchangeDbContext())
            {
                context.Transakcije.Add(transakcija);

                foreach (var finansijskaTransakcija in transakcija.FinansijskeTransakcije)
                {
                    var racun = context.RacuniBlagajne.FirstOrDefault(it => it.SifraValute.ToLower().Trim() == finansijskaTransakcija.SifraValuteRacunaBlagajne.ToLower().Trim());
                    if (racun == null)
                    {
                        DodajValidacionuGresku(validacioneGreske, "", "Ne postoji racun za valutu {finansijskaTransakcija.SifraValuteRacunaBlagajne}!");
                        if (validacija == false) throw new Exception("Ne postoji racun za valutu {finansijskaTransakcija.SifraValuteRacunaBlagajne}");
                    }
                    else
                    {
                        if (finansijskaTransakcija.Smer == SmerFinansijskeTransakcije.Izlaz &&
                            racun.Stanje < finansijskaTransakcija.Iznos &&
                            transakcija.FinansijskeTransakcije.Any(it => it.RacunBlagajne.SifraValute == racun.SifraValute && it.Smer == SmerFinansijskeTransakcije.Ulaz) == false)
                        {
                            DodajValidacionuGresku(validacioneGreske, "IznosIz", $"Nema dovoljno sredstava na računu {racun.SifraValute}!");
                            if (validacija == false) throw new Exception($"Nema dovoljno sredstava na računu {racun.SifraValute}!");
                        }
                        else
                        {
                            switch (finansijskaTransakcija.Smer)
                            {
                                case SmerFinansijskeTransakcije.Izlaz:
                                    racun.Stanje -= finansijskaTransakcija.Iznos;
                                    break;
                                default:
                                    racun.Stanje += finansijskaTransakcija.Iznos;
                                    break;
                            }
                        }
                    }
                }

                if (validacija == false)
                {
                    context.SaveChanges();
                }
            }
        }

        private static List<FinansijskaTransakcija> NapraviFinansijskeTransakcije(
            string sifraValuteOtkupa,
            decimal iznosOtkupa,
            string sifraValuteProdaje,
            decimal iznosProdaje,
            decimal? medjuIznos = null)
        {
            var rezultat = new List<FinansijskaTransakcija>();

            var sifraDomaceValute = VratiDomacuValutu();

            if (string.Equals(sifraValuteOtkupa, sifraDomaceValute, StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(sifraValuteProdaje, sifraDomaceValute, StringComparison.InvariantCultureIgnoreCase))
            {
                //2 fin transakcije
                rezultat.Add(new FinansijskaTransakcija
                {
                    Iznos = iznosOtkupa,
                    SifraValuteRacunaBlagajne = sifraValuteOtkupa,
                    Redosled = 0,
                    Smer = SmerFinansijskeTransakcije.Ulaz
                });

                rezultat.Add(new FinansijskaTransakcija
                {
                    Iznos = iznosProdaje,
                    SifraValuteRacunaBlagajne = sifraValuteProdaje,
                    Redosled = 1,
                    Smer = SmerFinansijskeTransakcije.Izlaz
                });
            }
            else
            {
                if (medjuIznos.HasValue == false)
                    throw new Exception("Međuisnos nije unet!!!");

                rezultat.Add(new FinansijskaTransakcija
                {
                    Iznos = iznosOtkupa,
                    SifraValuteRacunaBlagajne = sifraValuteOtkupa,
                    Redosled = 0,
                    Smer = SmerFinansijskeTransakcije.Ulaz
                });

                rezultat.Add(new FinansijskaTransakcija
                {
                    Iznos = medjuIznos.Value,
                    SifraValuteRacunaBlagajne = sifraDomaceValute,
                    Redosled = 1,
                    Smer = SmerFinansijskeTransakcije.Izlaz
                });

                rezultat.Add(new FinansijskaTransakcija
                {
                    Iznos = medjuIznos.Value,
                    SifraValuteRacunaBlagajne = sifraDomaceValute,
                    Redosled = 2,
                    Smer = SmerFinansijskeTransakcije.Ulaz
                });

                rezultat.Add(new FinansijskaTransakcija
                {
                    Iznos = iznosProdaje,
                    SifraValuteRacunaBlagajne = sifraValuteProdaje,
                    Redosled = 3,
                    Smer = SmerFinansijskeTransakcije.Izlaz
                });
            }

            return rezultat;
        }

        private static string VratiDomacuValutu()
        {
            string sifraDomaceValute;

            using (var context = new ExchangeDbContext())
            {
                var domacaValuta = context.Valute.FirstOrDefault(it => it.Domaca);
                if (domacaValuta != null)
                {
                    sifraDomaceValute = domacaValuta.Sifra;
                }
                else
                {
                    throw new Exception("Domaca valuta nije postavljena!!!");
                }
            }

            return sifraDomaceValute;
        }

        private static Valuta VratiValutu(string sifra)
        {
            Valuta valuta;

            using (var context = new ExchangeDbContext())
            {
                valuta = context.Valute.FirstOrDefault(it => it.Sifra == sifra);
            }

            return valuta;
        }

        private static StavkaKursneListe VratiStavkuKursneListe(string sifraDomaceValute, string valutaTransakcije)
        {
            StavkaKursneListe stavkaKursneListe;

            using (var context = new ExchangeDbContext())
            {
                stavkaKursneListe = context
                    .StavkeKursnihLista
                    .FirstOrDefault(it =>
                        it.SifraValuteKursneListe.ToLower().Trim() == sifraDomaceValute.ToLower().Trim() &&
                        it.SifraValutaStavke.ToLower().Trim() == valutaTransakcije.ToLower().Trim());
            }

            return stavkaKursneListe;
        }

        private static void DodajValidacionuGresku(IDictionary<string, List<string>> rezultat, string parametar, string greska)
        {
            var greske = new List<string>();

            if (rezultat.ContainsKey(parametar) == false)
            {
                rezultat.Add(parametar, greske);
            }
            else
            {
                greske = rezultat[parametar];
            }

            greske.Add(greska);
        }
    }

    public class OtkupCmd
    {
        public string SifraValuta { get; set; }

        public decimal Iznos { get; set; }

        public string Naziv { get; set; }

        public string Opis { get; set; }
    }

    public class ProdajaCmd
    {
        public string SifraValuta { get; set; }

        public decimal Iznos { get; set; }

        public string Naziv { get; set; }

        public string Opis { get; set; }
    }

    public class KonverzijaCmd
    {
        public string SifraValutaIz { get; set; }

        public string SifraValutaU { get; set; }

        public decimal Iznos { get; set; }

        public string Naziv { get; set; }

        public string Opis { get; set; }
    }

    public class UplataCmd
    {
        public string SifraValuta { get; set; }

        public decimal Iznos { get; set; }

        public string Naziv { get; set; }

        public string Opis { get; set; }
    }

    public class StornoCmd
    {
        public int IdTransakcije { get; set; }

        public string Opis { get; set; }
    }
}