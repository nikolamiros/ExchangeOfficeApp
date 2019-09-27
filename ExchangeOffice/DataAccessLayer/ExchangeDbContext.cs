using System.Data.Entity;
using ExchangeOffice.DataAccessLayer.Mappings;
using ExchangeOffice.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExchangeOffice.DataAccessLayer
{
    public class ExchangeDbContext : IdentityDbContext<ApplicationUser>
    {
        public ExchangeDbContext() : this("ExchangeConnection")
        {
        }

        public ExchangeDbContext(string connectionString) : base(connectionString, throwIfV1Schema: false)
        {
        }
        
        public static ExchangeDbContext Create()
        {
            return new ExchangeDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Entities maps
            modelBuilder.Configurations.Add(new FinansijskaTransakcijaMap());
            modelBuilder.Configurations.Add(new KursnaListaMap());
            modelBuilder.Configurations.Add(new MenjackaTransakcijaMap());
            modelBuilder.Configurations.Add(new BaznaTransakcijaMap());
            modelBuilder.Configurations.Add(new KorekcijaStanjaBlagajneMap());
            modelBuilder.Configurations.Add(new RacunBlagajneMap());
            modelBuilder.Configurations.Add(new StavkaKursneListeMap());
            modelBuilder.Configurations.Add(new ValutaMap());
        }

        public DbSet<FinansijskaTransakcija> FinansijskaTransakcije { get; set; }

        public DbSet<KursnaLista> KursneListe { get; set; }


        public DbSet<BaznaTransakcija> Transakcije { get; set; }

        public DbSet<MenjackaTransakcija> MenjackeTransakcije { get; set; }

        public DbSet<KorekcijaStanjaBlagajne> KorekcijaStanjaBlagajne { get; set; }


        public DbSet<RacunBlagajne> RacuniBlagajne { get; set; }

        public DbSet<StavkaKursneListe> StavkeKursnihLista { get; set; }

        public DbSet<Valuta> Valute { get; set; }
    }
}