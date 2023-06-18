using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CRYPTO_Burcea.DAL;

namespace CRYPTO_Burcea.BLL
{
    public class BLLOperatiiAR
    {
        CRYPTOEntities bd;

        // initializare bd entity, prin declararea metodei constructor a clasei
        public BLLOperatiiAR()
        {
            bd = new CRYPTOEntities();
        }


        // Metoda afisare operatii
        // operatii pentru un anumit portofel
        public IQueryable<OperatiuneAR> Afiseaza(Int64 idCPortofel)
        {
            return bd.OperatiuneARs.Where(c => c.IdContinutPortofel == idCPortofel).OrderByDescending(c => c.DataOperatiune);
        }

        public IQueryable<OperatiuneAR> AfiseazaToateOperatiile()
        {
            return bd.OperatiuneARs.OrderByDescending(c => c.DataOperatiune);
        }


        // Cautare operatiune
        public IQueryable<OperatiuneAR> Cauta(IQueryable<OperatiuneAR> sursadateinitilala, Expression<Func<OperatiuneAR, bool>> predicat)
        {
            return sursadateinitilala.Where(predicat);
        }


        // Adaugare operatiune
        public void Adauga(OperatiuneAR operatiune)
        {
            bd.OperatiuneARs.Add(operatiune);
            bd.SaveChanges();
        }

        // Modificare operatiune
        public void Modifica(OperatiuneAR operatiune)
        {
            bd.Entry(operatiune).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
        }

        // Sterge operatiune
        public void Sterge(Int64 idOperatiune)
        {
            OperatiuneAR operatiune = bd.OperatiuneARs.Find(idOperatiune);

            bd.OperatiuneARs.Remove(operatiune);
            bd.SaveChanges();
        }

        // Lista continut portofele
        public List<ContinutPortofel> ListaContinutPortofele(string username)
        {
            List<ContinutPortofel> rezultat = bd.ContinutPortofels.Where(c => c.Portofel.Utilizator.Username == username).OrderBy(c => c.Moneda.DenumireMoneda).ToList();
            return rezultat;

        }

    }
}