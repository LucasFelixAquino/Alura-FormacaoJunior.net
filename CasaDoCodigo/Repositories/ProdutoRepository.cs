﻿using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        private readonly ICategoriaRepository categoriaRepository;

        public ProdutoRepository(ApplicationContext contexto, ICategoriaRepository categoriaRepository) : base(contexto)
        {
            this.categoriaRepository = categoriaRepository;
        }

        public IList<Produto> GetProdutos()
        {
            return dbSet.Include(p => p.Categoria).ToList();
        }

        public IList<Produto> GetProdutos(string pesquisaProduto)
        {
            if (pesquisaProduto == null)
            {
                pesquisaProduto = "";
            }

            var produtosAchados = dbSet
                                     .Include(p => p.Categoria)
                                     .Where(p => p.Nome.ToUpper().Contains(pesquisaProduto.ToUpper()))
                                     .ToList();

            if (produtosAchados.Count() == 0)
            {
                return GetProdutos();
            }
            return produtosAchados;
        }

        public async Task SaveProdutos(List<Livro> livros)
        {

            foreach (var livro in livros)
            {
                var categoria = categoriaRepository.AddCategoria(livro.Categoria);

                if (!dbSet.Where(p => p.Codigo == livro.Codigo).Any())
                {
                    dbSet.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco, categoria));
                }
            }
            await contexto.SaveChangesAsync();
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public decimal Preco { get; set; }
    }
}
