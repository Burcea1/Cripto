using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRYPTO_Burcea.BLL;
using CRYPTO_Burcea.DAL;
using Microsoft.AspNet.Identity;


namespace CRYPTO_Burcea.Controllers
{
    public class UtilizatorController : Controller
    {
        BLLUtilizator oBLL = new BLLUtilizator();
        // GET: Utilizator

        // Actiune afisare utilizatori
        [Authorize]
        public ActionResult Index()
        {
            IQueryable<Utilizator> sursa = oBLL.Afiseaza();
            return View(sursa);
        }

        // Detalii Utilizator
        [Authorize]
        public ActionResult Detalii(Int64 idUtilizator)
        {
            Utilizator userdetalii = oBLL.Detalii(idUtilizator);
            return View(userdetalii);
        }

        // Editeaza utilizator - get(Afisare)
        [Authorize]
        public ActionResult Editeaza(Int64 idUtilizator)
        {
            Utilizator userdemodificat = oBLL.Detalii(idUtilizator);
            ViewBag.NumeUser = userdemodificat.Username;
            ViewBag.Nume = userdemodificat.Nume;
            ViewBag.CNP = userdemodificat.CNP;
            ViewBag.Adresa = userdemodificat.Adresa;
            ViewBag.Telefon = userdemodificat.Telefon;
            ViewBag.Email = userdemodificat.Email;
            ViewBag.Data = userdemodificat.DataCreare;
            return View(userdemodificat);
        }

        // Editeaza utilizator - post(Executare)
        [Authorize]
        [HttpPost]
        public ActionResult Editeaza(Utilizator userdemodificat)
        {
            oBLL.Modifica(userdemodificat);
            if (oBLL.EsteAdmin(User.Identity.GetUserName()))
            {
                return RedirectToAction("Index", "Utilizator");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }


        /*
        [Authorize]
        public ActionResult Adauga()
        {
            Utilizator usernou = new Utilizator();
            return View(usernou);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Adauga(Utilizator usernou)
        {
            oBLL.Adauga(usernou);
            return RedirectToAction("Index");
        }
        */

        // Stergere utilizator
        [Authorize]
        public ActionResult Sterge(Int64 idUtilizator)
        {
            CRYPTOEntities bd = new CRYPTOEntities();
            string user = bd.Utilizators.Where(u => u.Id == idUtilizator).Select(u => u.Username).FirstOrDefault();
            bool deconectare = true;
            if (oBLL.EsteAdmin(User.Identity.GetUserName()))
            {
                if (User.Identity.GetUserName() != user)
                {
                    deconectare = false;
                }
            }
            
            oBLL.Sterge(idUtilizator);
            if (deconectare)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Utilizator");
            }
            
        }
    }
}