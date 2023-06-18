using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CRYPTO_Burcea.DAL;

namespace CRYPTO_Burcea.BLL
{
    public class BLLTranzactionare
    {
        CRYPTOEntities bd;

        // initializare bd entity, prin declararea metodei constructor a clasei
        public BLLTranzactionare()
        {
            bd = new CRYPTOEntities();
        }


        // Metoda afisare tranzactionari
        public IQueryable<Tranzactionare> Afiseaza(string username)
        {
            return bd.Tranzactionares.Where(t => t.ContinutPortofel.Portofel.Utilizator.Username == username).OrderByDescending(t => t.DataTranzactionare);
        }

        public IQueryable<Tranzactionare> AfiseazaToateTranzactionarile()
        {
            return bd.Tranzactionares.OrderByDescending(t => t.DataTranzactionare);
        }

        // Cautare tranzactionare
        public IQueryable<Tranzactionare> Cauta(IQueryable<Tranzactionare> sursadateinitilala, Expression<Func<Tranzactionare, bool>> predicat)
        {
            return sursadateinitilala.Where(predicat);
        }


        // Adaugare tranzactionare
        public void Adauga(Tranzactionare tranzactionare)
        {
            bd.Tranzactionares.Add(tranzactionare);
            bd.SaveChanges();
        }

        // Modificare tranzactionare
        public void Modifica(Tranzactionare tranzactionare)
        {
            bd.Entry(tranzactionare).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
        }

        // Sterge tranzactionare
        public void Sterge(Int64 idTranzactionare)
        {
            Tranzactionare tranzactionare = bd.Tranzactionares.Find(idTranzactionare);

            bd.Tranzactionares.Remove(tranzactionare);
            bd.SaveChanges();
        }

        // Lista continut portofele 
        public List<ContinutPortofel> ListaContinutPortofele(string username)
        {
            List<ContinutPortofel> rezultat = bd.ContinutPortofels.Where(c => c.Portofel.Utilizator.Username == username).OrderBy(c => c.Moneda.DenumireMoneda).ToList();
            return rezultat;

        }

        // Lista monede
        public List<Moneda> ListaMonede(string username)
        {
            List<ContinutPortofel> cPortofele = bd.ContinutPortofels.Where(c => c.Portofel.Utilizator.Username == username).ToList();
            List<Moneda> rezultat = new List<Moneda>();
            foreach(ContinutPortofel cp in cPortofele)
            {
                Moneda moneda = bd.Monedas.Where(m => m.Id == cp.IdMoneda).FirstOrDefault();
                if (moneda != null)
                {
                    if (!rezultat.Contains(moneda))
                    {
                        rezultat.Add(moneda);
                    }
                }
            }
            return rezultat;

        }
    }
}