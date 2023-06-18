using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRYPTO_Burcea.DAL;
using CRYPTO_Burcea.BLL;
using Microsoft.AspNet.Identity;

namespace CRYPTO_Burcea.Controllers
{
    public class TranzactionareController : Controller
    {
        BLLTranzactionare oBLL = new BLLTranzactionare();

        // GET: Tranzactionare

        
        public ActionResult Index()
        {
            return View();
        }


        // Adauga tranzactionare - get(Afisare)
        [Authorize]
        public ActionResult Adauga(int idCPortofel, string tipT)
        {
            CRYPTOEntities bd = new CRYPTOEntities();
            ContinutPortofel cPortofel = bd.ContinutPortofels.Find(idCPortofel);
            ViewBag.idP = cPortofel.IdPortofel;
            ViewBag.tipT = tipT;
            ViewBag.Moneda = cPortofel.Moneda.DenumireMoneda;
            TempData["idCP"] = idCPortofel;
            TempData["idM"] = cPortofel.IdMoneda;
            TempData["tipT"] = tipT;
            TempData["moneda"] = cPortofel.Moneda.DenumireMoneda;
            TempData["idP"] = cPortofel.IdPortofel;

            Tranzactionare tranzactionarenoua = new Tranzactionare();
            if (tipT.Equals("Cumparare"))
            {
                tranzactionarenoua.IdContinutPortofelCumparare = idCPortofel;
                tranzactionarenoua.IdMonedaCumparata = cPortofel.IdMoneda;
                tranzactionarenoua.DataTranzactionare = DateTime.Today.Date;
                string username = User.Identity.GetUserName();
                tranzactionarenoua.ColectieContinutPortofeleC = oBLL.ListaContinutPortofele(username);
                tranzactionarenoua.ColectieMonedeC = oBLL.ListaMonede(username);


                List<ContinutPortofel> continutPortofele = oBLL.ListaContinutPortofele(username);
                int idMoneda = (int)cPortofel.IdMoneda;
                tranzactionarenoua.ColectieContinutPortofeleV = continutPortofele.Where(c => c.IdMoneda != idMoneda).ToList();

                List<Moneda> monede = oBLL.ListaMonede(username);
                Moneda mnd = monede.First(m => m.Id == idMoneda);
                monede.Remove(mnd);
                tranzactionarenoua.ColectieMonedeV = monede;
            }
            else if (tipT.Equals("Vanzare"))
            {
                tranzactionarenoua.IdContinutPortofelVanzare = idCPortofel;
                tranzactionarenoua.IdMonedaVanduta = cPortofel.IdMoneda;
                tranzactionarenoua.DataTranzactionare = DateTime.Today.Date;
                string username = User.Identity.GetUserName();
               

                List<ContinutPortofel> continutPortofele = oBLL.ListaContinutPortofele(username);
                int idMoneda = (int)cPortofel.IdMoneda;
                tranzactionarenoua.ColectieContinutPortofeleC = continutPortofele.Where(c => c.IdMoneda != idMoneda).ToList();

                List<Moneda> monede = oBLL.ListaMonede(username);
                Moneda mnd = monede.First(m => m.Id == idMoneda);
                monede.Remove(mnd);
                tranzactionarenoua.ColectieMonedeC = monede;

                tranzactionarenoua.ColectieContinutPortofeleV = oBLL.ListaContinutPortofele(username);
                tranzactionarenoua.ColectieMonedeV = oBLL.ListaMonede(username);
            }
            return View(tranzactionarenoua);
        }

        // Adauga tranzactionare - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult Adauga(Tranzactionare tranzactionarenoua)
        {
            int idCP = (int)TempData["idCP"];
            int idM = (int)TempData["idM"];
            string tipT = (string)TempData["tipT"];
            string denumiremoneda = (string)TempData["moneda"];
            int idP = (int)TempData["idP"];

            if (tipT.Equals("Cumparare"))
            {
                tranzactionarenoua.IdContinutPortofelCumparare = idCP;
                tranzactionarenoua.IdMonedaCumparata = idM;
                tranzactionarenoua.DataTranzactionare = DateTime.Today.Date;
            }
            else if (tipT.Equals("Vanzare"))
            {
                tranzactionarenoua.IdContinutPortofelVanzare = idCP;
                tranzactionarenoua.IdMonedaVanduta = idM;
                tranzactionarenoua.DataTranzactionare = DateTime.Today.Date;
            }

            if (ModelState.IsValid)
            {
                CRYPTOEntities bd = new CRYPTOEntities();
                ContinutPortofel cPortofel = bd.ContinutPortofels.Find(idCP);
                oBLL.Adauga(tranzactionarenoua);
                return RedirectToAction("Detalii", "Portofel", new { idPortofel = cPortofel.IdPortofel });
            }
            Tranzactionare tranzactionarenouadinnou = new Tranzactionare();
            if (tipT.Equals("Cumparare"))
            {
                tranzactionarenouadinnou.IdContinutPortofelCumparare = idCP;
                tranzactionarenouadinnou.IdMonedaCumparata = idM;
                tranzactionarenouadinnou.DataTranzactionare = DateTime.Today.Date;
                string username = User.Identity.GetUserName();
                tranzactionarenouadinnou.ColectieContinutPortofeleC = oBLL.ListaContinutPortofele(username);
                tranzactionarenouadinnou.ColectieMonedeC = oBLL.ListaMonede(username);


                List<ContinutPortofel> continutPortofele = oBLL.ListaContinutPortofele(username);
                int idMoneda = idM;
                tranzactionarenouadinnou.ColectieContinutPortofeleV = continutPortofele.Where(c => c.IdMoneda != idMoneda).ToList();

                List<Moneda> monede = oBLL.ListaMonede(username);
                Moneda mnd = monede.First(m => m.Id == idMoneda);
                monede.Remove(mnd);
                tranzactionarenouadinnou.ColectieMonedeV = monede;
            }
            else if (tipT.Equals("Vanzare"))
            {
                tranzactionarenouadinnou.IdContinutPortofelVanzare = idCP;
                tranzactionarenouadinnou.IdMonedaVanduta = idM;
                tranzactionarenouadinnou.DataTranzactionare = DateTime.Today.Date;
                string username = User.Identity.GetUserName();


                List<ContinutPortofel> continutPortofele = oBLL.ListaContinutPortofele(username);
                int idMoneda = idM;
                tranzactionarenouadinnou.ColectieContinutPortofeleC = continutPortofele.Where(c => c.IdMoneda != idMoneda).ToList();

                List<Moneda> monede = oBLL.ListaMonede(username);
                Moneda mnd = monede.First(m => m.Id == idMoneda);
                monede.Remove(mnd);
                tranzactionarenouadinnou.ColectieMonedeC = monede;

                tranzactionarenouadinnou.ColectieContinutPortofeleV = oBLL.ListaContinutPortofele(username);
                tranzactionarenouadinnou.ColectieMonedeV = oBLL.ListaMonede(username);
            }
            TempData["idCP"] = idCP;
            TempData["idM"] = idM;
            TempData["tipT"] = tipT;
            TempData["moneda"] = denumiremoneda;
            TempData["idP"] = idP;
            ViewBag.idP = idP;
            ViewBag.tipT = tipT;
            ViewBag.Moneda = denumiremoneda;
            return View(tranzactionarenouadinnou);
        }






        // Auto completare suma cumparata
        [Authorize]
        public JsonResult AutoCompletareSumaC(string sumaV, string idMC, string idMV)
        {
            int idMonedaCumparata = Convert.ToInt32(idMC);
            int idMonedaVanduta = Convert.ToInt32(idMV);
            decimal sumaVanduta = Convert.ToDecimal(sumaV) - (0.05m * Convert.ToDecimal(sumaV));  //---- COMISION 5%

            // Obtinem ultimul curs al monedei care se doreste a fi cumparata (curs de vanzare pt entitate)
            // si aflam suma cumparata din cealalta moneda
            CRYPTOEntities bd = new CRYPTOEntities();
            decimal curs = 0;
            decimal diferentaVanzare = 0;
            decimal sumaCumparata = 0;
            Moneda monedaCumparata = bd.Monedas.Where(m => m.Id == idMonedaCumparata).FirstOrDefault();
            Moneda monedaVanduta = bd.Monedas.Where(m => m.Id == idMonedaVanduta).FirstOrDefault();
            if (monedaCumparata.DenumireMoneda.Equals(monedaCumparata.Referinta))
            {
                curs = (decimal)bd.Curs.Where(c => c.IdMoneda == idMonedaVanduta).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).Select(c => c.Curs).FirstOrDefault();
                diferentaVanzare = (decimal)bd.Monedas.Where(m => m.Id == idMonedaVanduta).Select(m => m.DiferentaCumparare).FirstOrDefault();
                curs = Math.Round(curs * (1 + (diferentaVanzare / 100)), 4);

                // Aflare suma cumparata
                sumaCumparata = Math.Round(sumaVanduta * curs, 4);
            }
            else if (monedaVanduta.DenumireMoneda.Equals(monedaVanduta.Referinta))
            {
                curs = (decimal)bd.Curs.Where(c => c.IdMoneda == idMonedaCumparata).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).Select(c => c.Curs).FirstOrDefault();
                diferentaVanzare = (decimal)bd.Monedas.Where(m => m.Id == idMonedaCumparata).Select(m => m.DiferentaVanzare).FirstOrDefault();
                curs = Math.Round(curs * (1 + (diferentaVanzare / 100)), 4);

                // Aflare suma cumparata
                sumaCumparata = Math.Round(sumaVanduta / curs, 4);
            }
            else
            {
                decimal cursC = (decimal)bd.Curs.Where(c => c.IdMoneda == idMonedaCumparata).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).Select(c => c.Curs).FirstOrDefault();
                decimal cursV = (decimal)bd.Curs.Where(c => c.IdMoneda == idMonedaVanduta).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).Select(c => c.Curs).FirstOrDefault();
                curs = Math.Round(cursC / cursV, 4);
                diferentaVanzare = (decimal)bd.Monedas.Where(m => m.Id == idMonedaCumparata).Select(m => m.DiferentaVanzare).FirstOrDefault();
                curs = Math.Round(curs * (1 + (diferentaVanzare / 100)), 4);

                // Aflare suma cumparata
                sumaCumparata = Math.Round(sumaVanduta / curs, 4);
            }

            return Json(sumaCumparata, JsonRequestBehavior.AllowGet);
        }

        // Auto completare suma vanduta
        [Authorize]
        public JsonResult AutoCompletareSumaV(string sumaC, string idMC, string idMV)
        {
            int idMonedaCumparata = Convert.ToInt32(idMC);
            int idMonedaVanduta = Convert.ToInt32(idMV);
            decimal sumaCumparata = Convert.ToDecimal(sumaC);

            // Obtinem ultimul curs al monedei care se doreste a fi cumparata (curs de vanzare pt entitate)
            // si aflam suma vanduta din cealalta moneda
            CRYPTOEntities bd = new CRYPTOEntities();
            decimal curs = 0;
            decimal diferentaVanzare = 0;
            decimal sumaVanduta = 0;
            Moneda monedaCumparata = bd.Monedas.Where(m => m.Id == idMonedaCumparata).FirstOrDefault();
            Moneda monedaVanduta = bd.Monedas.Where(m => m.Id == idMonedaVanduta).FirstOrDefault();
            if (monedaCumparata.DenumireMoneda.Equals(monedaCumparata.Referinta))
            {
                curs = (decimal)bd.Curs.Where(c => c.IdMoneda == idMonedaVanduta).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).Select(c => c.Curs).FirstOrDefault();
                diferentaVanzare = (decimal)bd.Monedas.Where(m => m.Id == idMonedaVanduta).Select(m => m.DiferentaCumparare).FirstOrDefault();
                curs = Math.Round(curs * (1 + (diferentaVanzare / 100)), 4);

                // Aflare suma vanduta
                sumaVanduta = Math.Round(sumaCumparata / curs, 4);
            }
            else if (monedaVanduta.DenumireMoneda.Equals(monedaVanduta.Referinta))
            {
                curs = (decimal)bd.Curs.Where(c => c.IdMoneda == idMonedaCumparata).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).Select(c => c.Curs).FirstOrDefault();
                diferentaVanzare = (decimal)bd.Monedas.Where(m => m.Id == idMonedaCumparata).Select(m => m.DiferentaVanzare).FirstOrDefault();
                curs = Math.Round(curs * (1 + (diferentaVanzare / 100)), 4);

                // Aflare suma cumparata
                sumaVanduta = Math.Round(sumaCumparata * curs, 4);
            }
            else
            {
                decimal cursC = (decimal)bd.Curs.Where(c => c.IdMoneda == idMonedaCumparata).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).Select(c => c.Curs).FirstOrDefault();
                decimal cursV = (decimal)bd.Curs.Where(c => c.IdMoneda == idMonedaVanduta).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).Select(c => c.Curs).FirstOrDefault();
                curs = Math.Round(cursC / cursV, 4);
                diferentaVanzare = (decimal)bd.Monedas.Where(m => m.Id == idMonedaCumparata).Select(m => m.DiferentaVanzare).FirstOrDefault();
                curs = Math.Round(curs * (1 + (diferentaVanzare / 100)), 4);

                // Aflare suma cumparata
                sumaVanduta = Math.Round(sumaCumparata * curs, 4);
            }
            sumaVanduta = sumaVanduta - (0.05m * sumaVanduta);  //---- COMISION 5%
            return Json(sumaVanduta, JsonRequestBehavior.AllowGet);
        }

        // Auto completare comision
        [Authorize]
        public JsonResult AutoCompletareComision(string suma, string idM)
        {
            decimal comision = 0;
            decimal sumaV = Convert.ToDecimal(suma);
            int idMon = Convert.ToInt32(idM);
            CRYPTOEntities bd = new CRYPTOEntities();
            decimal curs = (decimal)bd.Curs.Where(c => c.IdMoneda == idMon).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).Select(c => c.Curs).FirstOrDefault();

            comision = 0.05m * sumaV;
            comision = comision * curs;
            comision = Math.Round(comision, 4);
            
            return Json(comision, JsonRequestBehavior.AllowGet);
        }

        // Lista de selectare a portofelelor in functie de moneda selectata
        [Authorize]
        public JsonResult ListaPortofeleChange(string idM, string idMa)
        {
            string username = User.Identity.GetUserName();
            List<ContinutPortofel> continutPortofele = oBLL.ListaContinutPortofele(username);
            if (!idM.Equals("") && !idM.Equals("0") && idM != null)
            {
                int idMon = Convert.ToInt32(idM);
                continutPortofele = continutPortofele.Where(c => c.IdMoneda == idMon).ToList();
            }
            else if (idM.Equals("") || idM.Equals("0") || idM == null)
            {
                int idMon = Convert.ToInt32(idMa);
                continutPortofele = continutPortofele.Where(c => c.IdMoneda != idMon).ToList();
            }

            IEnumerable<SelectListItem> continut = continutPortofele.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Portofel.DenumirePortofel
            });


            return Json(continut, JsonRequestBehavior.AllowGet);



        }


        // Verificare limite sume disponibile
        [Authorize]
        public JsonResult VerificareSume(string idCP)
        {
            CRYPTOEntities bd = new CRYPTOEntities();
            int idCp = Convert.ToInt32(idCP);
            ContinutPortofel cp = bd.ContinutPortofels.Find(idCp);
            decimal suma = (decimal)cp.SumaDetinuta;
            
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

            List<TransferICP> transferuri = bd.TransferICPs.Where(t => t.IdTransferatDin == idCp || t.IdTransferatIn == idCp).ToList();
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

            return Json(suma, JsonRequestBehavior.AllowGet);



        }
    }
}