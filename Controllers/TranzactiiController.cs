using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRYPTO_Burcea.DAL;
using CRYPTO_Burcea.BLL;
using Microsoft.AspNet.Identity;
using PagedList;

namespace CRYPTO_Burcea.Controllers
{
    public class TranzactiiController : Controller
    {
        BLLOperatiiAR oBLLo = new BLLOperatiiAR();
        BLLTransfer oBLLtrans = new BLLTransfer();
        BLLTranzactionare oBLLtranz = new BLLTranzactionare();

        // GET: Tranzactii

        // Afisare tranzactii - toate felurile
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // Afisare istoric alimentari / retrageri
        [Authorize]
        public ActionResult IstoricAR(string searchData, string searchPortofel, int? pagina)
        {
            string username = User.Identity.GetUserName();
            List<OperatiuneAR> sursadate;
            IQueryable<OperatiuneAR> sursa;
            sursa = oBLLo.AfiseazaToateOperatiile().Where(o => o.ContinutPortofel.Portofel.Utilizator.Username == username);
            if (!string.IsNullOrEmpty(searchData))
            {
                DateTime data = Convert.ToDateTime(searchData);
                sursa = oBLLo.Cauta(sursa, o => o.DataOperatiune == data);
            }
            if (!string.IsNullOrEmpty(searchPortofel))
            {
                sursa = oBLLo.Cauta(sursa, o => o.ContinutPortofel.Portofel.DenumirePortofel.Contains(searchPortofel));
            }

            sursadate = sursa.OrderByDescending(o => o.DataOperatiune).ToList();
            return PartialView("_IstoricAR", sursadate.ToPagedList(pagina ?? 1, 10));
        }

        // Afisare istoric transferuri
        [Authorize]
        public ActionResult IstoricTransferuri(string searchData, string searchPortofel, int? pagina)
        {
            string username = User.Identity.GetUserName();
            List<TransferICP> sursadate;
            IQueryable<TransferICP> sursa;
            sursa = oBLLtrans.AfiseazaTaoateTransferurile().Where(t => t.ContinutPortofel.Portofel.Utilizator.Username == username);
            if (!string.IsNullOrEmpty(searchData))
            {
                DateTime data = Convert.ToDateTime(searchData);
                sursa = oBLLtrans.Cauta(sursa, t => t.DataTransfer == data);
            }
            if (!string.IsNullOrEmpty(searchPortofel))
            {
                sursa = oBLLtrans.Cauta(sursa, t => t.ContinutPortofel.Portofel.DenumirePortofel.Contains(searchPortofel) || t.ContinutPortofel1.Portofel.DenumirePortofel.Contains(searchPortofel));
            }

            sursadate = sursa.OrderByDescending(t => t.DataTransfer).ToList();
            return PartialView("_IstoricTransferuri", sursadate.ToPagedList(pagina ?? 1, 10));
        }

        // Afisare istoric tranzactionari
        [Authorize]
        public ActionResult IstoricTranzactionari(string searchData, string searchPortofel, int? pagina)
        {
            string username = User.Identity.GetUserName();
            List<Tranzactionare> sursadate;
            IQueryable<Tranzactionare> sursa;
            sursa = oBLLtranz.AfiseazaToateTranzactionarile().Where(t => t.ContinutPortofel.Portofel.Utilizator.Username == username);
            if (!string.IsNullOrEmpty(searchData))
            {
                DateTime data = Convert.ToDateTime(searchData);
                sursa = oBLLtranz.Cauta(sursa, t => t.DataTranzactionare == data);
            }
            if (!string.IsNullOrEmpty(searchPortofel))
            {
                sursa = oBLLtranz.Cauta(sursa, t => t.ContinutPortofel.Portofel.DenumirePortofel.Contains(searchPortofel) || t.ContinutPortofel1.Portofel.DenumirePortofel.Contains(searchPortofel));
            }

            sursadate = sursa.OrderByDescending(t => t.DataTranzactionare).ToList();
            return PartialView("_IstoricTranzactionari", sursadate.ToPagedList(pagina ?? 1, 10));
        }
    }
}