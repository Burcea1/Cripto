using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRYPTO_Burcea.BLL;
using CRYPTO_Burcea.DAL;
using PagedList;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Web.Services;

namespace CRYPTO_Burcea.Controllers
{
    public class MonedaController : Controller
    {
        BLLMoneda oBLL = new BLLMoneda();
        BLLCursMoneda oBLLc = new BLLCursMoneda();
        BLLDisponibil oBLLd = new BLLDisponibil();
        BLLTranzactionare oBLLt = new BLLTranzactionare();

        // GET: Moneda

        // Actiune afisare monede
        [Authorize]
        public ActionResult Index(string searchDenumire, int? pagina)
        {
            List<Moneda> sursadate;
            IQueryable<Moneda> sursa;
            sursa = oBLL.Afiseaza();
            if (!string.IsNullOrEmpty(searchDenumire))
            {
                sursa = oBLL.Cauta(sursa, m => m.DenumireMoneda.Contains(searchDenumire));
            }

            sursadate = sursa.OrderBy(m => m.DenumireMoneda).ToList();
            return View(sursadate.ToPagedList(pagina ?? 1, 5));
        }

        // Detalii moneda
        [Authorize]
        public ActionResult Detalii(Int64 idMoneda)
        {
            Moneda monedadetalii = oBLL.Detalii(idMoneda);
            ViewBag.Moneda = monedadetalii.DenumireMoneda;


            // GRAFIC
            CRYPTOEntities bd = new CRYPTOEntities();
            var cursuri = bd.Monedas.Where(m => m.Id == idMoneda).Select(m => new { data = m.DataIntrare.ToString(), curs = m.CursIntrare }).ToList();
            var cursuriafter = bd.Curs.Where(c => c.IdMoneda == idMoneda).OrderBy(c => c.DataCurs).ThenBy(c => c.OraCurs).Select(c => new {data = c.DataCurs.ToString(), curs = c.Curs }).ToList();
            foreach(var curs in cursuriafter)
            {
                cursuri.Add(curs);
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(cursuri, _jsonSetting);

            return View(monedadetalii);
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        // Editeaza moneda - get(Afisare)
        [Authorize]
        public ActionResult Editeaza(Int64 idMoneda)
        {
            Moneda monedademodificat = oBLL.Detalii(idMoneda);
            ViewBag.Moneda = monedademodificat.DenumireMoneda;
            return View(monedademodificat);
        }

        // Editeaza moneda - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult Editeaza(Moneda monedademodificat)
        {
            oBLL.Modifica(monedademodificat);
            return RedirectToAction("Index", "Moneda");

        }

        // Adauga moneda - get(Afisare)
        [Authorize]
        public ActionResult Adauga()
        {
            List<string> referinte = new List<string>();
            referinte.Add("Euro");
            ViewBag.ListaR = referinte.Select(r => new SelectListItem()
            {
                Value = r,
                Text = r
            });
            Moneda monedanoua = new Moneda();
            return View(monedanoua);
        }

        // Adauga moneda - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult Adauga(Moneda monedanoua)
        {
            oBLL.Adauga(monedanoua);
            return RedirectToAction("Index", "Moneda");
        }

        // Stergere moneda
        [Authorize]
        public ActionResult Sterge(Int64 idMoneda)
        {
            oBLL.Sterge(idMoneda);
            return RedirectToAction("Index", "Moneda");
        }

        

        






        // Afisare cursuri
        [Authorize]
        public ActionResult IndexCurs(Int64 idMoneda, string searchData, int? pagina)
        {
            ViewBag.id = idMoneda;
            ViewBag.Diferente = oBLLc.DiferentaCurs(idMoneda);
            //IQueryable<Cur> sursa = oBLLc.Afiseaza(idMoneda);
            //return PartialView("_IndexCurs", sursa);


            List<Cur> sursadate;
            IQueryable<Cur> sursa;
            sursa = oBLLc.Afiseaza(idMoneda);
            if (!string.IsNullOrEmpty(searchData))
            {
                DateTime data = Convert.ToDateTime(searchData);
                sursa = oBLLc.Cauta(sursa, c => c.DataCurs == data);
            }
            sursadate = sursa.OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).ToList();

            return PartialView("_IndexCurs", sursadate.ToPagedList(pagina ?? 1, 5));
        }

        // Adauga curs - get(Afisare)
        [Authorize]
        public ActionResult AdaugaCurs(int idMoneda)
        {
            Cur cursnou = new Cur();
            cursnou.IdMoneda = idMoneda;
            cursnou.ColectieMonede = oBLLc.ListaMonede();
            return View(cursnou);
        }

        // Adauga curs - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult AdaugaCurs(Cur cursnou)
        {
            oBLLc.Adauga(cursnou);
            return RedirectToAction("Detalii", "Moneda", new { idMoneda = cursnou.IdMoneda});
        }

        // Stergere curs
        [Authorize]
        public ActionResult StergeCurs(Int64 idCurs)
        {
            CRYPTOEntities bd = new CRYPTOEntities();
            int idMoneda = (int)bd.Curs.Where(c => c.Id == idCurs).Select(c => c.IdMoneda).FirstOrDefault();
            oBLLc.Sterge(idCurs);
            return RedirectToAction("Detalii", "Moneda", new { idMoneda = idMoneda });
        }






        // Afisare dispnibil
        [Authorize]
        public ActionResult IndexDisponibil(Int64 idMoneda)
        {
            ViewBag.id = idMoneda;
            ViewBag.Disponibil = oBLL.SoldMoneda(idMoneda);
            IQueryable<Disponibil> sursa = oBLLd.Afiseaza(idMoneda);
            return PartialView("_IndexDisponibil", sursa);
        }

        // Editeaza disponibil - get(Afisare)
        [Authorize]
        public ActionResult EditeazaDisponibil(Int64 idDisponibil)
        {
            Disponibil disponibildemodificat = oBLLd.Detalii(idDisponibil);
            ViewBag.Moneda = disponibildemodificat.Moneda.DenumireMoneda;
            disponibildemodificat.DataModificare = DateTime.Today.Date;
            disponibildemodificat.OraModificare = DateTime.Now.TimeOfDay;
            disponibildemodificat.ColectieMonede = oBLLd.ListaMonede();
            return View(disponibildemodificat);
        }

        // Editeaza moneda - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult EditeazaDisponibil(Disponibil disponibildemodificat)
        {
            disponibildemodificat.DataModificare = DateTime.Today.Date;
            disponibildemodificat.OraModificare = DateTime.Now.TimeOfDay;
            oBLLd.Modifica(disponibildemodificat);
            return RedirectToAction("Detalii", "Moneda", new { idMoneda = disponibildemodificat.IdMoneda });

        }




        // Afisare dropdownbox de unde putem alege un portofel pentru care vom cumpara moneda
        [Authorize]
        public ActionResult IndexAlegePortofel(Int64 idMoneda)
        {
            ViewBag.id = idMoneda;
            string username = User.Identity.GetUserName();
            List<ContinutPortofel> continutPortofele = oBLLt.ListaContinutPortofele(username);
            ViewBag.Lista = continutPortofele.Where(c => c.IdMoneda == idMoneda).ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Portofel.DenumirePortofel
            });
            return PartialView("_IndexAlegePortofel");
        }



    }
}