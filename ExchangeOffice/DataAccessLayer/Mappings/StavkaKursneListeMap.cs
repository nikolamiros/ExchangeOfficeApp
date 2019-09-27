using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ExchangeOffice.Models;

namespace ExchangeOffice.DataAccessLayer.Mappings
{
    public class StavkaKursneListeMap : EntityTypeConfiguration<StavkaKursneListe>
    {
        public StavkaKursneListeMap()
        {
            ToTable(nameof(StavkaKursneListe), "dbo");

            HasKey(it => new { it.SifraValuteKursneListe, it.DatumKursneListe, it.SifraValutaStavke });

            HasRequired(it => it.ValutaStavke).WithMany().HasForeignKey(it => it.SifraValutaStavke);

            HasRequired(it => it.KursnaLista).WithMany(it => it.Stavke).HasForeignKey(it => new { it.SifraValuteKursneListe, it.DatumKursneListe });
        }
    }
}