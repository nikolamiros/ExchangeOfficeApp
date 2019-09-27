using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ExchangeOffice.DataAccessLayer;
using ExchangeOffice.ViewModels;

namespace ExchangeOffice.Controllers
{
    [Authorize]
    public class KursnaListaController : Controller
    {
        // GET: KursnaLista
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult VratiKursneListe()
        {

            var kursneListe = ExchangeRepository.VratiSveKursneListe();

            var result = kursneListe
                .OrderByDescending(it => it.Datum)
                .Select(it => new
                KursnaListaIndexViewModel
                {
                    SifraValute = it.SifraValute,
                    Datum = it.Datum.ToShortDateString(),
                    Opis = it.Opis
                }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VratiStavkeListe(string izabraniDatum,string izabranaValuta)
        {
            var stavkeKursneListe = ExchangeRepository.VratiStavkeKursneListe(izabraniDatum, izabranaValuta);

            var result = stavkeKursneListe.Select(it => new StavkeKursneListeIndexViewModel
            {
                Valuta = it.SifraValutaStavke,
                KupovniKurs = it.KupovniKurs.ToString(CultureInfo.CurrentCulture),
                ProdajniKurs = it.ProdajniKurs.ToString(CultureInfo.CurrentCulture),
                SrednjiKurs = it.SrednjiKurs.ToString(CultureInfo.CurrentCulture),
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult KreirajDanasnjuListu()
        {
            try
            {
                ExchangeRepository.KreirajDanasnjuKursnuListu();

                return new HttpStatusCodeResult(200);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
            }
        }

        [HttpPost]
        public ActionResult IzmeniStavkuKursneListe(StavkaKursneListeEditViewModel wm)
        {
            if (ModelState.IsValid == false)
            {
                Response.StatusCode = 400;
                return PartialView("_IzmeniStavkuPartial", wm);
            }

            try
            {
                ExchangeRepository.IzmeniStavkuKursneListe(wm.ValutaListe, wm.DatumListe, wm.Valuta, wm.KupovniKurs, wm.SrednjiKurs, wm.ProdajniKurs);

                return new HttpStatusCodeResult(200);
            }
            catch (Exception)
            {
                Response.StatusCode = 400;
                return PartialView("_IzmeniStavkuPartial", wm);
            }
        }
    }
}