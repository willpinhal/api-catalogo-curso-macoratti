using APICatalogo.Context;
using APICatalogo.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        
        public ProdutosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _appDbContext.Produtos.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _appDbContext.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            //Incorporado a partir da versão 2.1 dentro do Anotation [ApiController]
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //Adicinado na memória
            _appDbContext.Produtos.Add(produto);
            //Salva na base
            _appDbContext.SaveChanges();

            // Opção para retornar um response no post do recurso
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _appDbContext.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _appDbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            // Vai direto no banco buscar
            var produto = _appDbContext.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            
            // Verifica na memória antes de ir no banco
            //var produto = _appDbContext.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            _appDbContext.Produtos.Remove(produto);
            _appDbContext.SaveChanges();
            return produto;
        }
    }
}
