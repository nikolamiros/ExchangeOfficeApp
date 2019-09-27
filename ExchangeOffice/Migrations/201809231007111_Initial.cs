namespace ExchangeOffice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinansijskaTransakcija",
                c => new
                    {
                        TransakcijaId = c.Int(nullable: false),
                        Redosled = c.Int(nullable: false),
                        Iznos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Smer = c.Int(nullable: false),
                        SifraValuteRacunaBlagajne = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TransakcijaId, t.Redosled })
                .ForeignKey("dbo.RacunBlagajne", t => t.SifraValuteRacunaBlagajne, cascadeDelete: true)
                .ForeignKey("dbo.BaznaTransakcija", t => t.TransakcijaId, cascadeDelete: true)
                .Index(t => t.TransakcijaId)
                .Index(t => t.SifraValuteRacunaBlagajne);
            
            CreateTable(
                "dbo.RacunBlagajne",
                c => new
                    {
                        SifraValute = c.String(nullable: false, maxLength: 128),
                        Stanje = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Opis = c.String(),
                    })
                .PrimaryKey(t => t.SifraValute)
                .ForeignKey("dbo.Valuta", t => t.SifraValute)
                .Index(t => t.SifraValute);
            
            CreateTable(
                "dbo.Valuta",
                c => new
                    {
                        Sifra = c.String(nullable: false, maxLength: 128),
                        Naziv = c.String(),
                        Domaca = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Sifra);
            
            CreateTable(
                "dbo.BaznaTransakcija",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Stornirana = c.Boolean(nullable: false),
                        DatumTransakcije = c.DateTime(nullable: false),
                        Naziv = c.String(),
                        Opis = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KursnaLista",
                c => new
                    {
                        SifraValute = c.String(nullable: false, maxLength: 128),
                        Datum = c.DateTime(nullable: false),
                        Opis = c.String(),
                    })
                .PrimaryKey(t => new { t.SifraValute, t.Datum })
                .ForeignKey("dbo.Valuta", t => t.SifraValute, cascadeDelete: true)
                .Index(t => t.SifraValute);
            
            CreateTable(
                "dbo.StavkaKursneListe",
                c => new
                    {
                        SifraValuteKursneListe = c.String(nullable: false, maxLength: 128),
                        DatumKursneListe = c.DateTime(nullable: false),
                        SifraValutaStavke = c.String(nullable: false, maxLength: 128),
                        KupovniKurs = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProdajniKurs = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SrednjiKurs = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.SifraValuteKursneListe, t.DatumKursneListe, t.SifraValutaStavke })
                .ForeignKey("dbo.Valuta", t => t.SifraValutaStavke, cascadeDelete: true)
                .ForeignKey("dbo.KursnaLista", t => new { t.SifraValuteKursneListe, t.DatumKursneListe })
                .Index(t => new { t.SifraValuteKursneListe, t.DatumKursneListe })
                .Index(t => t.SifraValutaStavke);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.KorekcijaStanjaBlagajne",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SifraValute = c.String(nullable: false, maxLength: 128),
                        Iznos = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaznaTransakcija", t => t.Id)
                .ForeignKey("dbo.Valuta", t => t.SifraValute)
                .Index(t => t.Id)
                .Index(t => t.SifraValute);
            
            CreateTable(
                "dbo.MenjackaTransakcija",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SifraValutaOtkupa = c.String(nullable: false, maxLength: 128),
                        IznosOtkupa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SifraValutaProdaje = c.String(nullable: false, maxLength: 128),
                        IznosProdaje = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tip = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaznaTransakcija", t => t.Id)
                .ForeignKey("dbo.Valuta", t => t.SifraValutaOtkupa)
                .ForeignKey("dbo.Valuta", t => t.SifraValutaProdaje)
                .Index(t => t.Id)
                .Index(t => t.SifraValutaOtkupa)
                .Index(t => t.SifraValutaProdaje);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenjackaTransakcija", "SifraValutaProdaje", "dbo.Valuta");
            DropForeignKey("dbo.MenjackaTransakcija", "SifraValutaOtkupa", "dbo.Valuta");
            DropForeignKey("dbo.MenjackaTransakcija", "Id", "dbo.BaznaTransakcija");
            DropForeignKey("dbo.KorekcijaStanjaBlagajne", "SifraValute", "dbo.Valuta");
            DropForeignKey("dbo.KorekcijaStanjaBlagajne", "Id", "dbo.BaznaTransakcija");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.KursnaLista", "SifraValute", "dbo.Valuta");
            DropForeignKey("dbo.StavkaKursneListe", new[] { "SifraValuteKursneListe", "DatumKursneListe" }, "dbo.KursnaLista");
            DropForeignKey("dbo.StavkaKursneListe", "SifraValutaStavke", "dbo.Valuta");
            DropForeignKey("dbo.FinansijskaTransakcija", "TransakcijaId", "dbo.BaznaTransakcija");
            DropForeignKey("dbo.FinansijskaTransakcija", "SifraValuteRacunaBlagajne", "dbo.RacunBlagajne");
            DropForeignKey("dbo.RacunBlagajne", "SifraValute", "dbo.Valuta");
            DropIndex("dbo.MenjackaTransakcija", new[] { "SifraValutaProdaje" });
            DropIndex("dbo.MenjackaTransakcija", new[] { "SifraValutaOtkupa" });
            DropIndex("dbo.MenjackaTransakcija", new[] { "Id" });
            DropIndex("dbo.KorekcijaStanjaBlagajne", new[] { "SifraValute" });
            DropIndex("dbo.KorekcijaStanjaBlagajne", new[] { "Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.StavkaKursneListe", new[] { "SifraValutaStavke" });
            DropIndex("dbo.StavkaKursneListe", new[] { "SifraValuteKursneListe", "DatumKursneListe" });
            DropIndex("dbo.KursnaLista", new[] { "SifraValute" });
            DropIndex("dbo.RacunBlagajne", new[] { "SifraValute" });
            DropIndex("dbo.FinansijskaTransakcija", new[] { "SifraValuteRacunaBlagajne" });
            DropIndex("dbo.FinansijskaTransakcija", new[] { "TransakcijaId" });
            DropTable("dbo.MenjackaTransakcija");
            DropTable("dbo.KorekcijaStanjaBlagajne");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.StavkaKursneListe");
            DropTable("dbo.KursnaLista");
            DropTable("dbo.BaznaTransakcija");
            DropTable("dbo.Valuta");
            DropTable("dbo.RacunBlagajne");
            DropTable("dbo.FinansijskaTransakcija");
        }
    }
}
