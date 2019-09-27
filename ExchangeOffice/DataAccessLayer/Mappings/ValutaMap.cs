using System.Data.Entity.ModelConfiguration;
using ExchangeOffice.Models;

namespace ExchangeOffice.DataAccessLayer.Mappings
{
    public class ValutaMap : EntityTypeConfiguration<Valuta>
    {
        public ValutaMap()
        {
            ToTable(nameof(Valuta), "dbo");

            HasKey(it => it.Sifra);
        }
    }
}