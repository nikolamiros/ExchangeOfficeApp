using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ExchangeOffice.Business;
using ExchangeOffice.DataAccessLayer;
using ExchangeOffice.ViewModels;

namespace ExchangeOffice.Controllers
{
    [Authorize]
    public class BlagajnaController : Controller
    {
        // GET: Blagajna
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Uplata()
        {
            ModelState.Clear();
            return PartialView("_UplataPartial", new UplataViewModel());
        }

        [HttpPost]
        public ActionResult Uplata(UplataViewModel wm)
        {
            if (ModelState.IsValid == false)
            {
                Response.StatusCode = 400;
                return PartialView("_UplataPartial", wm);
            }

            ExchangeEngine.Izvrsi(new UplataCmd()
            {
                Iznos = wm.Iznos.Value,
                SifraValuta = wm.SifraValuta
            });

            return new HttpStatusCodeResult(200);
        }
        
        public JsonResult VratiRacune()
        {
            var racuni = ExchangeRepository.VratiSveRacuneBlagajne();

            var result = racuni.Select(it => new
            {
                it.SifraValute,
                Stanje = it.Stanje.ToString(CultureInfo.InvariantCulture),
                it.Opis
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}