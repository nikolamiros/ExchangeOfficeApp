using System.Data.Entity.ModelConfiguration;
using ExchangeOffice.Models;

namespace ExchangeOffice.DataAccessLayer.Mappings
{
    public class BaznaTransakcijaMap : EntityTypeConfiguration<BaznaTransakcija>
    {
        public BaznaTransakcijaMap()
        {
            ToTable(nameof(BaznaTransakcija), "dbo");

            HasKey(it => it.Id);
            
            HasMany(it => it.FinansijskeTransakcije).WithRequired(it => it.Transakcija);
        }
    }
}