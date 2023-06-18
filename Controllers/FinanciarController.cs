using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRYPTO_Burcea.BLL;
using CRYPTO_Burcea.DAL;
using PagedList;

namespace CRYPTO_Burcea.Controllers
{
    public class FinanciarController : Controller
    {
        BLLFinanciar oBLL = new BLLFinanciar();
        // GET: Financiar

        // Afisare interfata pentru alegerea monedei
        [Authorize]
        public ActionResult Index()
        {
            CRYPTOEntities bd = new CRYPTOEntities();
            List<Moneda> monede = bd.Monedas.ToList();
            ViewBag.ListaMonede = monede.Select(m => new SelectListItem()
            {
                Value = m.Id.ToString(),
                Text = m.DenumireMoneda
            });
            return View();
        }

        // Detalii
        [Authorize]
        public ActionResult Detalii(Int64 idMoneda)
        {
            ViewBag.idMoneda = idMoneda;
            return PartialView("_Detalii");
        }

        // Afisare vanzari societate (cumparari client)
        [Authorize]
        public ActionResult Vanzari(Int64 idMoneda, string searchData, int? pagina)
        {
            ViewBag.id = idMoneda;
            List<Tranzactionare> sursadate;
            IQueryable<Tranzactionare> sursa;
            sursa = oBLL.Vanzari(idMoneda);

            decimal suma = 0;
            decimal comisioane = 0;
            foreach (Tranzactionare t in sursa)
            {
                suma += (decimal)t.SumaCumparata;
                comisioane += (decimal)t.ComisionEuro;
            }

            suma = Math.Round(suma, 2);
            comisioane = Math.Round(comisioane, 2);
            ViewBag.suma = suma;
            ViewBag.comisioane = comisioane;

            if (!string.IsNullOrEmpty(searchData))
            {
                DateTime data = Convert.ToDateTime(searchData);
                sursa = oBLL.Cauta(sursa, t => t.DataTranzactionare == data);
            }

            sursadate = sursa.OrderByDescending(t => t.DataTranzactionare).ToList();
            return PartialView("_Vanzari", sursadate.ToPagedList(pagina ?? 1, 10));
        }

        // Afisare cumparari societate (vanzari client)
        [Authorize]
        public ActionResult Cumparari(Int64 idMoneda, string searchData, int? pagina)
        {
            ViewBag.id = idMoneda;
            List<Tranzactionare> sursadate;
            IQueryable<Tranzactionare> sursa;
            sursa = oBLL.Cumparari(idMoneda);

            decimal suma = 0;
            decimal comisioane = 0;
            foreach (Tranzactionare t in sursa)
            {
                suma += (decimal)t.SumaVanduta;
                comisioane += (decimal)t.ComisionEuro;
            }

            suma = Math.Round(suma, 2);
            comisioane = Math.Round(comisioane, 2);
            ViewBag.suma = suma;
            ViewBag.comisioane = comisioane;

            if (!string.IsNullOrEmpty(searchData))
            {
                DateTime data = Convert.ToDateTime(searchData);
                sursa = oBLL.Cauta(sursa, t => t.DataTranzactionare == data);
            }

            sursadate = sursa.OrderByDescending(t => t.DataTranzactionare).ToList();
            return PartialView("_Cumparari", sursadate.ToPagedList(pagina ?? 1, 10));
        }

    }
}