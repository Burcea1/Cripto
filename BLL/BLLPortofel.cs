using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CRYPTO_Burcea.DAL;

namespace CRYPTO_Burcea.BLL
{
    public class BLLPortofel
    {
        CRYPTOEntities bd;

        // initializare bd entity, prin declararea metodei constructor a clasei
        public BLLPortofel()
        {
            bd = new CRYPTOEntities();
        }


        // Metoda afisare portofel
        // Portofelele utilizatorului curent
        public IQueryable<Portofel> Afiseaza(string username)
        {
            int id = bd.Utilizators.Where(u => u.Username == username).Select(u => u.Id).FirstOrDefault();
            return bd.Portofels.Where(p => p.IdUtilizator == id);
        }

        // Cautare portofel
        public IQueryable<Portofel> Cauta(IQueryable<Portofel> sursadateinitilala, Expression<Func<Portofel, bool>> predicat)
        {
            return sursadateinitilala.Where(predicat);
        }

        // Detalii Portofel
        public Portofel Detalii(Int64 idPortofel)
        {
            Portofel rezultat;
            rezultat = bd.Portofels.Find(idPortofel);
            return rezultat;
        }

        // Adaugare Portofel
        public void Adauga(Portofel portofel)
        {
            bd.Portofels.Add(portofel);
            bd.SaveChanges();
        }

        // Modificare Portofel
        public void Modifica(Portofel portofel)
        {
            bd.Entry(portofel).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
        }

        // Sterge Portofel
        public void Sterge(Int64 idPortofel)
        {
            List<ContinutPortofel> cportofel = bd.ContinutPortofels.Where(cp => cp.IdPortofel == idPortofel).ToList();
            foreach (ContinutPortofel cport in cportofel)
            {
                bd.ContinutPortofels.Remove(cport);
            }
            Portofel portofel = bd.Portofels.Find(idPortofel);
            bd.Portofels.Remove(portofel);
            bd.SaveChanges();
        }

        // Lista utilizatori
        public List<Utilizator> ListaUtilizatori()
        {
            List<Utilizator> rezultat = bd.Utilizators.OrderBy(u => u.Nume).ToList();
            return rezultat;

        }
    }
}