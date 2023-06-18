using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CRYPTO_Burcea.DAL;

namespace CRYPTO_Burcea.BLL
{
    public class BLLContinutPortofel
    {
        CRYPTOEntities bd;

        // initializare bd entity, prin declararea metodei constructor a clasei
        public BLLContinutPortofel()
        {
            bd = new CRYPTOEntities();
        }


        // Metoda afisare continut portofel
        // Continut portofel pentru un anumit portofel
        public IQueryable<ContinutPortofel> Afiseaza(Int64 idPortofel)
        {
            return bd.ContinutPortofels.Where(c => c.IdPortofel == idPortofel).OrderByDescending(c => c.DataDeschidere).ThenBy(c => c.Moneda.DenumireMoneda);
        }

        // Cautare Continut portofel
        public IQueryable<ContinutPortofel> Cauta(IQueryable<ContinutPortofel> sursadateinitilala, Expression<Func<ContinutPortofel, bool>> predicat)
        {
            return sursadateinitilala.Where(predicat);
        }


        // Adaugare continut portofel
        public void Adauga(ContinutPortofel cPortofel)
        {
            bd.ContinutPortofels.Add(cPortofel);
            bd.SaveChanges();
        }

        // Modificare continut portofel
        public void Modifica(ContinutPortofel cPortofel)
        {
            bd.Entry(cPortofel).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
        }

        // Sterge continut portofel
        public void Sterge(Int64 idCPortofel)
        {
            ContinutPortofel cPortofel = bd.ContinutPortofels.Find(idCPortofel);

            bd.ContinutPortofels.Remove(cPortofel);
            bd.SaveChanges();
        }

        // Lista monede
        public List<Moneda> ListaMonede()
        {
            List<Moneda> rezultat = bd.Monedas.OrderBy(m => m.DenumireMoneda).ToList();
            return rezultat;

        }

        // Lista portofele
        public List<Portofel> ListaPortofele(string username)
        {
            List<Portofel> rezultat = bd.Portofels.Where(p => p.Utilizator.Username == username).OrderBy(p => p.DenumirePortofel).ToList();
            return rezultat;

        }
    }
}