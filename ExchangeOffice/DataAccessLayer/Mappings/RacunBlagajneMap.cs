using System.Data.Entity.ModelConfiguration;
using ExchangeOffice.Models;

namespace ExchangeOffice.DataAccessLayer.Mappings
{
    public class RacunBlagajneMap : EntityTypeConfiguration<RacunBlagajne>
    {
        public RacunBlagajneMap()
        {
            ToTable(nameof(RacunBlagajne), "dbo");

            HasKey(it => it.SifraValute);

            HasRequired(it => it.Valuta).WithOptional();
        }
    }
}