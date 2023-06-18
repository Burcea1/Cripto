using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CRYPTO_Burcea.DAL;


namespace CRYPTO_Burcea.BLL
{
    public class BLLTransfer
    {
        CRYPTOEntities bd;

        // initializare bd entity, prin declararea metodei constructor a clasei
        public BLLTransfer()
        {
            bd = new CRYPTOEntities();
        }


        // Metoda afisare transferuri
        // transferuri pentru un anumit continut portofel
        public IQueryable<TransferICP> Afiseaza(Int64 idCPortofel)
        {
            return bd.TransferICPs.Where(t => t.IdTransferatDin == idCPortofel || t.IdTransferatIn == idCPortofel).OrderByDescending(t => t.DataTransfer);
        }

        public IQueryable<TransferICP> AfiseazaTaoateTransferurile()
        {
            return bd.TransferICPs.OrderByDescending(t => t.DataTransfer);
        }

        // Cautare transfer
        public IQueryable<TransferICP> Cauta(IQueryable<TransferICP> sursadateinitilala, Expression<Func<TransferICP, bool>> predicat)
        {
            return sursadateinitilala.Where(predicat);
        }


        // Adaugare transfer
        public void Adauga(TransferICP transfer)
        {
            bd.TransferICPs.Add(transfer);
            bd.SaveChanges();
        }

        // Modificare transfer
        public void Modifica(TransferICP transfer)
        {
            bd.Entry(transfer).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
        }

        // Sterge transfer
        public void Sterge(Int64 idTransfer)
        {
            TransferICP transfer = bd.TransferICPs.Find(idTransfer);

            bd.TransferICPs.Remove(transfer);
            bd.SaveChanges();
        }

        // Lista continut portofele 
        public List<ContinutPortofel> ListaContinutPortofele(string username, int idMoneda)
        {
            List<ContinutPortofel> rezultat = bd.ContinutPortofels.Where(c => c.Portofel.Utilizator.Username == username && c.IdMoneda == idMoneda).OrderBy(c => c.Moneda.DenumireMoneda).ToList();
            return rezultat;

        }


    }
}