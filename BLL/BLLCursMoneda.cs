using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CRYPTO_Burcea.DAL;

namespace CRYPTO_Burcea.BLL
{
    public class BLLCursMoneda
    {
        CRYPTOEntities bd;

        // initializare bd entity, prin declararea metodei constructor a clasei
        public BLLCursMoneda()
        {
            bd = new CRYPTOEntities();
        }


        // Metoda afisare cursuri
        // Cursuri pentru o anumita moneda
        public IQueryable<Cur> Afiseaza(Int64 idMoneda)
        {
            return bd.Curs.Where(c => c.IdMoneda == idMoneda).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs);
        }

        // Cautare curs
        public IQueryable<Cur> Cauta(IQueryable<Cur> sursadateinitilala, Expression<Func<Cur, bool>> predicat)
        {
            return sursadateinitilala.Where(predicat);
        }


        // Adaugare curs
        public void Adauga(Cur curs)
        {
            bd.Curs.Add(curs);
            bd.SaveChanges();
        }

        // Sterge curs
        public void Sterge(Int64 idCurs)
        {
            Cur curs = bd.Curs.Find(idCurs);

            bd.Curs.Remove(curs);
            bd.SaveChanges();
        }

        // Lista monede
        public List<Moneda> ListaMonede()
        {
            List<Moneda> rezultat = bd.Monedas.OrderBy(m => m.DenumireMoneda).ToList();
            return rezultat;

        }


        // Crestere sau scadere curs moneda
        public List<decimal> DiferentaCurs(Int64 idMoneda)
        {
            decimal diferenta = 0;
            decimal cursnou = 0;
            decimal cursvechi = 0;
            List<decimal> diferente = new List<decimal>();
            List<Cur> cursuri = bd.Curs.Where(c => c.IdMoneda == idMoneda).OrderByDescending(c => c.DataCurs).ThenByDescending(c => c.OraCurs).ToList();
            if (cursuri != null && cursuri.Count() >= 1)
            {
                for (int i = 0; i < cursuri.Count(); i++)
                {
                    if (i < cursuri.Count() - 1)
                    {
                        cursnou = (decimal)cursuri[i].Curs;
                        cursvechi = (decimal)cursuri[i + 1].Curs;
                    }
                    else 
                    {
                        cursnou = (decimal)cursuri[i].Curs;
                        cursvechi = (decimal)cursuri[i].Moneda.CursIntrare;
                    }

                    if (cursnou > cursvechi)
                    {
                        diferenta = cursnou - cursvechi;
                    }
                    else if (cursvechi > cursnou)
                    {
                        diferenta = cursnou - cursvechi;
                    }

                    diferente.Add(diferenta);

                }


            }

            return diferente;
        }
    }
}