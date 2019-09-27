using System;
using System.Collections.Generic;

namespace ExchangeOffice.Models
{
    public class Valuta
    {
        public string Sifra { get; set; }

        public string Naziv { get; set; }

        public bool Domaca { get; set; }
    }

    public class RacunBlagajne 
    {
        public string SifraValute { get; set; }

        public decimal Stanje { get; set; }

        public string Opis { get; set; }

        public virtual Valuta Valuta { get; set; }
    }

    public class MenjackaTransakcija : BaznaTransakcija
    {
        public string SifraValutaOtkupa { get; set; }

        public decimal IznosOtkupa { get; set; }
        
        public string SifraValutaProdaje { get; set; }

        public decimal IznosProdaje { get; set; }
        
        public TipMenjackeTransakcije Tip { get; set; }
        
        public virtual Valuta ValutaOtkupa { get; set; }

        public virtual Valuta ValutaProdaje { get; set; }
    }

    public class KorekcijaStanjaBlagajne : BaznaTransakcija
    {
        public string SifraValute { get; set; }

        public decimal Iznos { get; set; }

        public virtual Valuta Valuta { get; set; }        
    }

    public abstract class BaznaTransakcija
    {
        public int Id { get; set; }

        public bool Stornirana { get; set; }

        public DateTime DatumTransakcije { get; set; }

        public string Naziv { get; set; }

        public string Opis { get; set; }

        public virtual ICollection<FinansijskaTransakcija> FinansijskeTransakcije { get; set; }
    }

    public class FinansijskaTransakcija
    {
        public int TransakcijaId { get; set; }

        public int Redosled { get; set; }
        
        public decimal Iznos { get; set; }

        public SmerFinansijskeTransakcije Smer { get; set; }

        public string SifraValuteRacunaBlagajne { get; set; }

        public virtual RacunBlagajne RacunBlagajne { get; set; }

        public virtual BaznaTransakcija Transakcija { get; set; }
    }
    
    public class KursnaLista
    {
        public string SifraValute{ get; set; }

        public DateTime Datum { get; set; }
        
        public string  Opis { get; set; }

        public virtual ICollection<StavkaKursneListe> Stavke { get; set; }

        public virtual Valuta Valuta { get; set; }
    }

    public class StavkaKursneListe
    {
        public string SifraValuteKursneListe { get; set; }

        public DateTime DatumKursneListe { get; set; }

        public string SifraValutaStavke { get; set; }

        public decimal KupovniKurs { get; set; }

        public decimal ProdajniKurs { get; set; }

        public decimal SrednjiKurs { get; set; }

        public virtual Valuta ValutaStavke { get; set; }

        public virtual KursnaLista KursnaLista { get; set; }
    }

    public enum SmerFinansijskeTransakcije
    {
        Ulaz = 0,
        Izlaz = 1
    }

    public enum TipMenjackeTransakcije
    {
        Otkup,
        Prodaja,
        Konverzija
    }

}