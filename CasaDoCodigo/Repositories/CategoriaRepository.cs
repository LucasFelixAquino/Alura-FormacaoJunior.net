﻿using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface ICategoriaRepository
    {
        Categoria AddCategoria(string nome);
    }
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {


        public CategoriaRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public Categoria AddCategoria(string nome)
        {
            var categoria = dbSet.FirstOrDefault(c => c.Nome == nome);
            if (categoria == null)
            {
                categoria = new Categoria { Nome = nome };
                dbSet.Add(categoria);
                contexto.SaveChanges();
            }
            return categoria;
            
        }
    }
}