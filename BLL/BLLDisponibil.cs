using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRYPTO_Burcea.DAL;

namespace CRYPTO_Burcea.BLL
{
    public class BLLDisponibil
    {
        CRYPTOEntities bd;

        // initializare bd entity, prin declararea metodei constructor a clasei
        public BLLDisponibil()
        {
            bd = new CRYPTOEntities();
        }


        // Metoda afisare disponibil
        // Disponibil pentru o anumita moneda
        public IQueryable<Disponibil> Afiseaza(Int64 idMoneda)
        {
            return bd.Disponibils.Where(d => d.IdMoneda == idMoneda);
        }

        // Detalii Disponibil
        public Disponibil Detalii(Int64 idDisponibil)
        {
            Disponibil rezultat;
            rezultat = bd.Disponibils.Find(idDisponibil);
            return rezultat;
        }

        // Adaugare disponibil
        public void Adauga(Disponibil disponibil)
        {
            bd.Disponibils.Add(disponibil);
            bd.SaveChanges();
        }

        // Modifica disponibil
        public void Modifica(Disponibil disponibil)
        {
            bd.Entry(disponibil).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
        }

        // Sterge disponibil
        public void Sterge(Int64 idDisponibil)
        {
            Disponibil disponibil = bd.Disponibils.Find(idDisponibil);

            bd.Disponibils.Remove(disponibil);
            bd.SaveChanges();
        }

        // Lista monede
        public List<Moneda> ListaMonede()
        {
            List<Moneda> rezultat = bd.Monedas.OrderBy(m => m.DenumireMoneda).ToList();
            return rezultat;

        }
    }
}