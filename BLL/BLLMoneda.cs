using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CRYPTO_Burcea.DAL;

namespace CRYPTO_Burcea.BLL
{
    public class BLLMoneda
    {
        CRYPTOEntities bd;

        // initializare bd entity, prin declararea metodei constructor a clasei
        public BLLMoneda()
        {
            bd = new CRYPTOEntities();
        }


        // Metoda afisare monede
        // Toate monedele
        public IQueryable<Moneda> Afiseaza()
        {
            return bd.Monedas;
        }

        // Cautare monede
        public IQueryable<Moneda> Cauta(IQueryable<Moneda> sursadateinitilala, Expression<Func<Moneda, bool>> predicat)
        {
            return sursadateinitilala.Where(predicat);
        }

        // Detalii Moneda
        public Moneda Detalii(Int64 idMoneda)
        {
            Moneda rezultat;
            rezultat = bd.Monedas.Find(idMoneda);
            return rezultat;
        }

        // Adaugare moneda
        public void Adauga(Moneda moneda)
        {
            bd.Monedas.Add(moneda);
            bd.SaveChanges();
            int id = bd.Monedas.Where(m => m.DenumireMoneda == moneda.DenumireMoneda && m.DataIntrare == moneda.DataIntrare).Select(m => m.Id).FirstOrDefault();
            Disponibil disponibil = new Disponibil();
            disponibil.IdMoneda = id;
            disponibil.Disponibil1 = 0;
            disponibil.DataModificare = DateTime.Today.Date;
            disponibil.OraModificare = DateTime.Now.TimeOfDay;
            bd.Disponibils.Add(disponibil);
            bd.SaveChanges();
        }

        // Modificare moneda
        public void Modifica(Moneda moneda)
        {
            bd.Entry(moneda).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
        }

        // Sterge moneda
        public void Sterge(Int64 idMoneda)
        {
            Disponibil disponibil = bd.Disponibils.Where(d => d.IdMoneda == idMoneda).FirstOrDefault();
            bd.Disponibils.Remove(disponibil);
            List<Cur> cursuri = bd.Curs.Where(c => c.IdMoneda == idMoneda).ToList();
            foreach(Cur curs in cursuri)
            {
                bd.Curs.Remove(curs);
            }
            Moneda moneda = bd.Monedas.Find(idMoneda);
            bd.Monedas.Remove(moneda);
            bd.SaveChanges();
        }


        // Calculare sold Moneda
        public decimal SoldMoneda(Int64 idMoneda)
        {
            decimal sold = 0;
            decimal disponibil = (decimal)bd.Disponibils.Where(d => d.IdMoneda == idMoneda).Select(d => d.Disponibil1).FirstOrDefault();
            sold += disponibil;
            List<Tranzactionare> tranzactionari = bd.Tranzactionares.Where(t => t.IdMonedaCumparata == idMoneda || t.IdMonedaVanduta == idMoneda).ToList();
            foreach (Tranzactionare tranz in tranzactionari)
            {
                if (tranz.IdMonedaVanduta == idMoneda)
                {
                    sold += (decimal)tranz.SumaVanduta;
                }
                else if (tranz.IdMonedaCumparata == idMoneda)
                {
                    sold -= (decimal)tranz.SumaCumparata;
                }
            }
            return sold;
        }
    }
}