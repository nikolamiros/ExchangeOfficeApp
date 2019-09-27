using System.Data.Entity.ModelConfiguration;
using ExchangeOffice.Models;

namespace ExchangeOffice.DataAccessLayer.Mappings
{
    public class KorekcijaStanjaBlagajneMap : EntityTypeConfiguration<KorekcijaStanjaBlagajne>
    {
        public KorekcijaStanjaBlagajneMap()
        {
            ToTable(nameof(KorekcijaStanjaBlagajne), "dbo");

            HasRequired(it => it.Valuta).WithMany().HasForeignKey(it => it.SifraValute)
                .WillCascadeOnDelete(false); ;
        }
    }
}