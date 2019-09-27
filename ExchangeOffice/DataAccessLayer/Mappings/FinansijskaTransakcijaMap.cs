using System.Data.Entity.ModelConfiguration;
using ExchangeOffice.Models;

namespace ExchangeOffice.DataAccessLayer.Mappings
{
    public class FinansijskaTransakcijaMap : EntityTypeConfiguration<FinansijskaTransakcija>
    {
        public FinansijskaTransakcijaMap()
        {
            ToTable(nameof(FinansijskaTransakcija), "dbo");

            HasKey(it => new { it.TransakcijaId, it.Redosled });

            HasRequired(it => it.Transakcija)
                .WithMany(it => it.FinansijskeTransakcije)
                .HasForeignKey(it => it.TransakcijaId);

            HasRequired(it => it.RacunBlagajne).WithMany()
                .HasForeignKey(it => it.SifraValuteRacunaBlagajne);
        }
    }
}