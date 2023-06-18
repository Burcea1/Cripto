using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRYPTO_Burcea.BLL;
using CRYPTO_Burcea.DAL;
using PagedList;
using Microsoft.AspNet.Identity;

namespace CRYPTO_Burcea.Controllers
{
    public class PortofelController : Controller
    {
        BLLPortofel oBLL = new BLLPortofel();
        BLLContinutPortofel oBLLc = new BLLContinutPortofel();
        BLLOperatiiAR oBLLo = new BLLOperatiiAR();
        BLLTransfer oBLLt = new BLLTransfer();

        // GET: Portofel

        // Actiune afisare portofele
        [Authorize]
        public ActionResult Index(string searchDenumire, int? pagina)
        {
            List<Portofel> sursadate;
            IQueryable<Portofel> sursa;
            string username = User.Identity.GetUserName();
            sursa = oBLL.Afiseaza(username);
            if (!string.IsNullOrEmpty(searchDenumire))
            {
                sursa = oBLL.Cauta(sursa, p => p.DenumirePortofel.Contains(searchDenumire));
            }

            sursadate = sursa.OrderBy(p => p.DenumirePortofel).ToList();
            return View(sursadate.ToPagedList(pagina ?? 1, 5));
        }

        // Detalii portofel
        [Authorize]
        public ActionResult Detalii(Int64 idPortofel)
        {
            Portofel portofeldetalii = oBLL.Detalii(idPortofel);
            ViewBag.Portofel = portofeldetalii.DenumirePortofel;
            return View(portofeldetalii);
        }

        // Editeaza portofel - get(Afisare)
        [Authorize]
        public ActionResult Editeaza(Int64 idPortofel)
        {
            Portofel portofeldemodificat = oBLL.Detalii(idPortofel);
            ViewBag.Portofel = portofeldemodificat.DenumirePortofel;
            portofeldemodificat.ColectieUtilizatori = oBLL.ListaUtilizatori();
            return View(portofeldemodificat);
        }

        // Editeaza portofel - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult Editeaza(Portofel portofeldemodificat)
        {
            oBLL.Modifica(portofeldemodificat);
            return RedirectToAction("Index", "Portofel");

        }

        // Adauga portofel - get(Afisare)
        [Authorize]
        public ActionResult Adauga()
        {
            string username = User.Identity.GetUserName();
            CRYPTOEntities bd = new CRYPTOEntities();
            int idU = bd.Utilizators.Where(u => u.Username == username).Select(u => u.Id).FirstOrDefault();
            TempData["id"] = idU;
            Portofel portofelnou = new Portofel();
            portofelnou.IdUtilizator = idU;
            portofelnou.DataCreare = DateTime.Today.Date;
            portofelnou.ColectieUtilizatori = oBLL.ListaUtilizatori();
            return View(portofelnou);
        }

        // Adauga portofel - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult Adauga(Portofel portofelnou)
        {
            portofelnou.DataCreare = DateTime.Today.Date;
            int idU = (int)TempData["id"];
            portofelnou.IdUtilizator = idU;
            oBLL.Adauga(portofelnou);
            return RedirectToAction("Index", "Portofel");
        }

        // Stergere portofel
        [Authorize]
        public ActionResult Sterge(Int64 idPortofel)
        {
            oBLL.Sterge(idPortofel);
            return RedirectToAction("Index", "Portofel");
        }






        // Afisare continut portofel
        [Authorize]
        public ActionResult IndexContinutPortofel(Int64 idPortofel, string searchData, string searchMoneda, int? pagina)
        {
            ViewBag.id = idPortofel;
            //IQueryable<Portofel> sursa = oBLLc.Afiseaza(idMoneda);
            //return PartialView("_IndexContinutPortofel", sursa);


            List<ContinutPortofel> sursadate;
            IQueryable<ContinutPortofel> sursa;
            sursa = oBLLc.Afiseaza(idPortofel);
            if (!string.IsNullOrEmpty(searchData))
            {
                DateTime data = Convert.ToDateTime(searchData);
                sursa = oBLLc.Cauta(sursa, c => c.DataDeschidere == data);
            }
            if (!string.IsNullOrEmpty(searchMoneda))
            {
                sursa = oBLLc.Cauta(sursa, c => c.Moneda.DenumireMoneda.Contains(searchMoneda));
            }
            sursadate = sursa.OrderByDescending(c => c.DataDeschidere).ThenBy(c => c.Moneda.DenumireMoneda).ToList();

            List<string> sume = new List<string>();
            foreach (ContinutPortofel cp in sursadate)
            {
                int idCp = cp.Id;
                decimal suma = (decimal)cp.SumaDetinuta;
                CRYPTOEntities bd = new CRYPTOEntities();
                List<OperatiuneAR> operatiuni = bd.OperatiuneARs.Where(o => o.IdContinutPortofel == idCp).ToList();
                foreach (OperatiuneAR op in operatiuni)
                {
                    if (op.TipOperatiune.Equals("Alimentare"))
                    {
                        suma += (decimal)op.SumaOperatiune;
                    }
                    else if (op.TipOperatiune.Equals("Retragere"))
                    {
                        suma -= (decimal)op.SumaOperatiune;
                    }
                }

                List<TransferICP> transferuri = bd.TransferICPs.Where(t => t.IdTransferatDin == idCp || t.IdTransferatIn ==  idCp).ToList();
                foreach (TransferICP trans in transferuri)
                {
                    if (trans.IdTransferatDin == idCp)
                    {
                        suma -= (decimal)trans.SumaTransferata;
                    }
                    else if (trans.IdTransferatIn == idCp)
                    {
                        suma += (decimal)trans.SumaTransferata;
                    }
                }

                List<Tranzactionare> tranzactionari = bd.Tranzactionares.Where(t => t.IdContinutPortofelCumparare == idCp || t.IdContinutPortofelVanzare == idCp).ToList();
                foreach (Tranzactionare tranz in tranzactionari)
                {
                    if (tranz.IdContinutPortofelCumparare == idCp)
                    {
                        suma += (decimal)tranz.SumaCumparata;
                    }
                    else if (tranz.IdContinutPortofelVanzare == idCp)
                    {
                        suma -= (decimal)tranz.SumaVanduta;
                    }
                }

                sume.Add(suma.ToString());
            }
            ViewBag.ListaSume = sume;

            return PartialView("_IndexContinutPortofel", sursadate.ToPagedList(pagina ?? 1, 5));
        }

        // Adauga continut portofel - get(Afisare)
        [Authorize]
        public ActionResult AdaugaContinutPortofel(int idPortofel)
        {
            TempData["id"] = idPortofel;
            ContinutPortofel cPortofelnou = new ContinutPortofel();
            cPortofelnou.IdPortofel = idPortofel;
            cPortofelnou.DataDeschidere = DateTime.Today.Date;
            cPortofelnou.SumaDetinuta = 0;
            cPortofelnou.ColectieMonede = oBLLc.ListaMonede();
            string username = User.Identity.GetUserName();
            cPortofelnou.ColectiePortofele = oBLLc.ListaPortofele(username);
            return View(cPortofelnou);
        }

        // Adauga continut portofel - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult AdaugaContinutPortofel(ContinutPortofel cPortofelnou)
        {
            int idP = (int)TempData["id"];
            cPortofelnou.IdPortofel = idP;
            cPortofelnou.DataDeschidere = DateTime.Today.Date;
            oBLLc.Adauga(cPortofelnou);
            return RedirectToAction("Detalii", "Portofel", new { idPortofel = cPortofelnou.IdPortofel });
        }

        // Stergere continut portofel
        [Authorize]
        public ActionResult StergeContinutPortofel(Int64 idCPortofel)
        {
            CRYPTOEntities bd = new CRYPTOEntities();
            int idPortofel = (int)bd.ContinutPortofels.Where(c => c.Id == idCPortofel).Select(c => c.IdPortofel).FirstOrDefault();
            oBLLc.Sterge(idCPortofel);
            return RedirectToAction("Detalii", "Portofel", new { idPortofel = idPortofel });
        }





        // Adauga operatiune - get(Afisare)
        [Authorize]
        public ActionResult AdaugaOperatiune(int idCPortofel, string tipO)
        {
            CRYPTOEntities bd = new CRYPTOEntities();
            ContinutPortofel cPortofel = bd.ContinutPortofels.Find(idCPortofel);
            ViewBag.idP = cPortofel.IdPortofel;
            TempData["id"] = idCPortofel;
            TempData["tipO"] = tipO;
            OperatiuneAR operatiunenoua = new OperatiuneAR();
            operatiunenoua.IdContinutPortofel = idCPortofel;
            operatiunenoua.DataOperatiune = DateTime.Today.Date;
            operatiunenoua.TipOperatiune = tipO;
            string username = User.Identity.GetUserName();
            operatiunenoua.ColectieContinutPortofele = oBLLo.ListaContinutPortofele(username);
            return View(operatiunenoua);
        }

        // Adauga operatiune - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult AdaugaOperatiune(OperatiuneAR operatiunenoua)
        {
            int idCP = (int)TempData["id"];
            string tipO = (string)TempData["tipO"];
            operatiunenoua.IdContinutPortofel = idCP;
            operatiunenoua.DataOperatiune = DateTime.Today.Date;
            operatiunenoua.TipOperatiune = tipO;
            CRYPTOEntities bd = new CRYPTOEntities();
            ContinutPortofel cPortofel = bd.ContinutPortofels.Find(idCP);
            oBLLo.Adauga(operatiunenoua);
            return RedirectToAction("Detalii", "Portofel", new { idPortofel = cPortofel.IdPortofel });
        }







        // Adauga transfer - get(Afisare)
        [Authorize]
        public ActionResult AdaugaTransfer(int idCPortofel)
        {
            CRYPTOEntities bd = new CRYPTOEntities();
            ContinutPortofel cPortofel = bd.ContinutPortofels.Find(idCPortofel);
            ViewBag.idP = cPortofel.IdPortofel;
            ViewBag.Moneda = cPortofel.Moneda.DenumireMoneda;
            TempData["id"] = idCPortofel;
            TransferICP transfernou = new TransferICP();
            transfernou.IdTransferatDin = idCPortofel;
            transfernou.DataTransfer = DateTime.Today.Date;
            string username = User.Identity.GetUserName();
            int idMoneda = (int)cPortofel.IdMoneda;
            transfernou.ColectieContinutPortofeleDin = oBLLt.ListaContinutPortofele(username, idMoneda);
            List<ContinutPortofel> continutPortofele = oBLLt.ListaContinutPortofele(username, idMoneda);
            ContinutPortofel cp = continutPortofele.First(c => c.Id == idCPortofel);
            continutPortofele.Remove(cp);
            transfernou.ColectieContinutPortofeleIn = continutPortofele;
            return View(transfernou);
        }

        // Adauga transfer - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult AdaugaTransfer(TransferICP transfernou)
        {
            int idCP = (int)TempData["id"];
            transfernou.IdTransferatDin = idCP;
            transfernou.DataTransfer = DateTime.Today.Date;
            CRYPTOEntities bd = new CRYPTOEntities();
            ContinutPortofel cPortofel = bd.ContinutPortofels.Find(idCP);
            oBLLt.Adauga(transfernou);
            return RedirectToAction("Detalii", "Portofel", new { idPortofel = cPortofel.IdPortofel });
        }
    }
}