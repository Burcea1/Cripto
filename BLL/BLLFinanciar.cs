using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CRYPTO_Burcea.DAL;

namespace CRYPTO_Burcea.BLL
{
    public class BLLFinanciar
    {
        CRYPTOEntities bd;

        // initializare bd entity, prin declararea metodei constructor a clasei
        public BLLFinanciar()
        {
            bd = new CRYPTOEntities();
        }


        // Afisare vanzari de moneda
        public IQueryable<Tranzactionare> Vanzari(Int64 idMoneda)
        {
            return bd.Tranzactionares.Where(t => t.IdMonedaCumparata == idMoneda).OrderByDescending(t => t.DataTranzactionare);
        }

        // Afisare cumparari de moneda
        public IQueryable<Tranzactionare> Cumparari(Int64 idMoneda)
        {
            return bd.Tranzactionares.Where(t => t.IdMonedaVanduta == idMoneda).OrderByDescending(t => t.DataTranzactionare);
        }

        // Cautare 
        public IQueryable<Tranzactionare> Cauta(IQueryable<Tranzactionare> sursadateinitilala, Expression<Func<Tranzactionare, bool>> predicat)
        {
            return sursadateinitilala.Where(predicat);
        }

    }
}