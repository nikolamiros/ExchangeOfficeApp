using System.Data.Entity.ModelConfiguration;
using ExchangeOffice.Models;

namespace ExchangeOffice.DataAccessLayer.Mappings
{
    public class MenjackaTransakcijaMap : EntityTypeConfiguration<MenjackaTransakcija>
    {
        public MenjackaTransakcijaMap()
        {
            ToTable(nameof(MenjackaTransakcija), "dbo");

            HasRequired(it => it.ValutaOtkupa).WithMany().HasForeignKey(it => it.SifraValutaOtkupa)
                .WillCascadeOnDelete(false); ;

            HasRequired(it => it.ValutaProdaje).WithMany().HasForeignKey(it => it.SifraValutaProdaje)
                .WillCascadeOnDelete(false); ;
        }
    }
}