using System.Data.Entity.ModelConfiguration;
using ExchangeOffice.Models;

namespace ExchangeOffice.DataAccessLayer.Mappings
{
    public class KursnaListaMap : EntityTypeConfiguration<KursnaLista>
    {
        public KursnaListaMap()
        {
            ToTable(nameof(KursnaLista), "dbo");

            HasKey(it => new { it.SifraValute, it.Datum });

            HasRequired(it => it.Valuta).WithMany().HasForeignKey(it => it.SifraValute);

            HasMany(it => it.Stavke).WithRequired(it => it.KursnaLista).WillCascadeOnDelete(false);
        }
    }
}