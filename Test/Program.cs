using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeOffice.Business;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //ExchangeEngine.Izvrsi(new UplataCmd
            //{
            //    Iznos = 100000,
            //    SifraValuta = "RSD"
            //});


            var validacioniProblemi =  ExchangeEngine.Validiraj(new OtkupCmd
            {
                Iznos = 100000,
                SifraValuta = "EUR"
            });
        }
    }
}
