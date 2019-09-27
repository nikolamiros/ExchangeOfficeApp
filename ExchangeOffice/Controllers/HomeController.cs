using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ExchangeOffice.Business;
using ExchangeOffice.DataAccessLayer;
using ExchangeOffice.ViewModels;

namespace ExchangeOffice.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Konverzija()
        {
            ModelState.Clear();
            return PartialView("_KonverzijaPartial", new KonverzijaViewModel());
        }
        
        [HttpPost]
        public ActionResult Konverzija(KonverzijaViewModel wm)
        {
            if (ModelState.IsValid == false)
            {
                Response.StatusCode = 400;
                return PartialView("_KonverzijaPartial", wm);
            }

            ExchangeEngine.Izvrsi(new KonverzijaCmd
            {
                Iznos = wm.IznosIz.Value,
                SifraValutaIz = wm.SifraValutaIz,
                Naziv = wm.Naziv,
                SifraValutaU = wm.SifraValutaU,
                Opis = wm.Opis
            });

            return new HttpStatusCodeResult(200);
        }

        public ActionResult IzracunajIznosKonverzije(KonverzijaViewModel wm)
        {
            var rezultat = (decimal)0;

            if (wm.IznosIz.HasValue)
            {
                rezultat = ExchangeEngine.Izracunaj(new KonverzijaCmd
                {
                    Iznos = wm.IznosIz.Value,
                    SifraValutaIz = wm.SifraValutaIz,
                    SifraValutaU = wm.SifraValutaU,
                });
            }

            return Json(rezultat, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Otkup()
        {
            ModelState.Clear();
            return PartialView("_OtkupPartial", new OtkupViewModel());
        }

        [HttpPost]
        public ActionResult Otkup(OtkupViewModel wm)
        {
            if (ModelState.IsValid == false)
            {
                Response.StatusCode = 400;
                return PartialView("_OtkupPartial", wm);
            }

            ExchangeEngine.Izvrsi(new OtkupCmd()
            {
                Iznos = wm.Iznos.Value,
                SifraValuta = wm.SifraValuta
            });

            return new HttpStatusCodeResult(200);
        }

        public ActionResult IzracunajIznosOtkupa(OtkupViewModel wm)
        {
            var rezultat = (decimal)0;

            if (wm.Iznos.HasValue)
            {
                rezultat = ExchangeEngine.Izracunaj(new OtkupCmd
                {
                    Iznos = wm.Iznos.Value,
                    SifraValuta = wm.SifraValuta
                });
            }

            return Json(rezultat, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Prodaja()
        {
            ModelState.Clear();
            return PartialView("_ProdajaPartial", new ProdajaViewModel());
        }

        [HttpPost]
        public ActionResult Prodaja(ProdajaViewModel wm)
        {
            if (ModelState.IsValid == false)
            {
                Response.StatusCode = 400;
                return PartialView("_ProdajaPartial", wm);
            }

            ExchangeEngine.Izvrsi(new ProdajaCmd()
            {
                Iznos = wm.Iznos.Value,
                SifraValuta = wm.SifraValuta
            });

            return new HttpStatusCodeResult(200);
        }

        public ActionResult IzracunajIznosProdaje(ProdajaViewModel wm)
        {
            var rezultat = (decimal)0;

            if (wm.Iznos.HasValue)
            {
                rezultat = ExchangeEngine.Izracunaj(new ProdajaCmd
                {
                    Iznos = wm.Iznos.Value,
                    SifraValuta = wm.SifraValuta
                });
            }

            return Json(rezultat, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Storniranje(int? idTransakcijeZaStorniranje)
        {
            ExchangeRepository.StornirajTransackiju(idTransakcijeZaStorniranje.Value);

            return new HttpStatusCodeResult(200);
        }


        public JsonResult VratiMenjackeTransakcije()
        {
            var transakcije = ExchangeRepository.VratiSveMenjackeTransakcije(false);

            var result = transakcije.Select(it =>
            {
                switch (it.Tip)
                {
                    case Models.TipMenjackeTransakcije.Otkup:
                        return new
                        {
                            it.Id,
                            DatumTransakcije = it.DatumTransakcije.ToString(),
                            Tip = it.Tip.ToString(),
                            Iznos = it.IznosOtkupa.ToString(CultureInfo.InvariantCulture),
                            Valuta = it.SifraValutaOtkupa.ToString()
                        };
                    case Models.TipMenjackeTransakcije.Prodaja:
                        return new
                        {
                            it.Id,
                            DatumTransakcije = it.DatumTransakcije.ToString(),
                            Tip = it.Tip.ToString(),
                            Iznos = it.IznosProdaje.ToString(CultureInfo.InvariantCulture),
                            Valuta = it.SifraValutaProdaje.ToString()
                        };
                    case Models.TipMenjackeTransakcije.Konverzija:
                        return new
                        {
                            it.Id,
                            DatumTransakcije = it.DatumTransakcije.ToString(),
                            Tip = it.Tip.ToString(),
                            Iznos = it.IznosOtkupa.ToString(CultureInfo.InvariantCulture)+"/"+ it.IznosProdaje.ToString(CultureInfo.InvariantCulture),
                            Valuta = it.SifraValutaOtkupa.ToString() + "/" + it.SifraValutaProdaje.ToString()
                        };
                    default: throw new ArgumentOutOfRangeException();
                }
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VratiStorniraneMenjackeTransakcije()
        {
            var transakcije = ExchangeRepository.VratiSveMenjackeTransakcije(true);

            var result = transakcije.Select(it =>
            {
                switch (it.Tip)
                {
                    case Models.TipMenjackeTransakcije.Otkup:
                        return new
                        {
                            it.Id,
                            DatumTransakcije = it.DatumTransakcije.ToString(),
                            Tip = it.Tip.ToString(),
                            Iznos = it.IznosOtkupa.ToString(CultureInfo.InvariantCulture),
                            Valuta = it.SifraValutaOtkupa.ToString()
                        };
                    case Models.TipMenjackeTransakcije.Prodaja:
                        return new
                        {
                            it.Id,
                            DatumTransakcije = it.DatumTransakcije.ToString(),
                            Tip = it.Tip.ToString(),
                            Iznos = it.IznosProdaje.ToString(CultureInfo.InvariantCulture),
                            Valuta = it.SifraValutaProdaje.ToString()
                        };
                    case Models.TipMenjackeTransakcije.Konverzija:
                        return new
                        {
                            it.Id,
                            DatumTransakcije = it.DatumTransakcije.ToString(),
                            Tip = it.Tip.ToString(),
                            Iznos = it.IznosOtkupa.ToString(CultureInfo.InvariantCulture) + "/" + it.IznosProdaje.ToString(CultureInfo.InvariantCulture),
                            Valuta = it.SifraValutaOtkupa.ToString() + "/" + it.SifraValutaProdaje.ToString()
                        };
                    default: throw new ArgumentOutOfRangeException();
                }
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
    }
}