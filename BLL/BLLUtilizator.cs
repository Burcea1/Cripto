using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CRYPTO_Burcea.DAL;
using Microsoft.AspNet.Identity;

namespace CRYPTO_Burcea.BLL
{
    public class BLLUtilizator
    {
        CRYPTOEntities bd;

        // initializare bd entity, prin declararea metodei constructor a clasei
        public BLLUtilizator()
        {
            bd = new CRYPTOEntities();
        }

        // Metoda aflare calitate utilizator :  admin sau simplu user
        public bool EsteAdmin(string username)
        {
            bool? calitateadmin = bd.Utilizators.Where(u => u.Username == username).Select(u => u.CalitateAdmin).FirstOrDefault();
            if (calitateadmin == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Metoda afisare utilizatori
        // Toti utilizatorii - admin, altfel: doar utilizatorul curent
        public IQueryable<Utilizator> Afiseaza() 
        {
            string user = HttpContext.Current.User.Identity.GetUserName();
            if (EsteAdmin(user))
            {
                return bd.Utilizators;
            }
            else 
            {
                return bd.Utilizators.Where(u => u.Username == user);
            }
        }

        // Cautare utilizatori
        public IQueryable<Utilizator> Cauta(IQueryable<Utilizator> sursadateinitilala, Expression<Func<Utilizator, bool>> predicat)
        {
            return sursadateinitilala.Where(predicat);
        }

        // Detalii utilizator
        public Utilizator Detalii(Int64 idUtilizator)
        {
            Utilizator rezultat;
            rezultat = bd.Utilizators.Find(idUtilizator);
            return rezultat;
        }

        // Adaugare utilizator
        public void Adauga(Utilizator utilizator)
        {
            bd.Utilizators.Add(utilizator);
            bd.SaveChanges();
        }

        // Modificare utilizator
        public void Modifica(Utilizator utilizator)
        {
            bd.Entry(utilizator).State = System.Data.Entity.EntityState.Modified;
            AspNetUser aspu = bd.AspNetUsers.Where(u => u.UserName == utilizator.Username).FirstOrDefault();
            aspu.Email = utilizator.Email;
            bd.Entry(aspu).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
        }

        // Sterge utilizator
        public void Sterge(Int64 idUtilizator)
        {
            Utilizator utilizator = bd.Utilizators.Find(idUtilizator);
            AspNetUser utilizatorAPP;
            string username = bd.Utilizators.Where(u => u.Id == idUtilizator).Select(u => u.Username).FirstOrDefault();
            utilizatorAPP = bd.AspNetUsers.Where(u => u.UserName == username).FirstOrDefault();
            bd.AspNetUsers.Remove(utilizatorAPP);
            bd.Utilizators.Remove(utilizator);
            bd.SaveChanges();
        }

    }
}