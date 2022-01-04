using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Domain;
using APICatalogo.Context;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CategoriaController(AppDbContext context)
        {
            this._appDbContext = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                return _appDbContext.Categorias.Include(x => x.Produtos).ToList();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter as categorias no banco de dados.");
            }
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                Categoria categoria = _appDbContext.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria com id {id} não foi encontrada.");
                }

                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter a categoria no banco de dados.");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return this._appDbContext.Categorias.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter os dados do banco.");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                this._appDbContext.Add(categoria);
                this._appDbContext.SaveChanges();

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar criar um nova categoria");
            }
        }

        [HttpPut]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest($"Não foi possível atualizar a categoria com id={id}");
                }

                this._appDbContext.Entry(categoria).State = EntityState.Modified;
                this._appDbContext.SaveChanges();

                return Ok($"Categoria com id={id} foi atualiza com sucesso.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar a categoria {id}.");
            }
        }

        [HttpDelete]
        public ActionResult<Categoria> Delete(int id)
        {

            try
            {
                var categoria = this._appDbContext.Categorias.Find(id);

                if (categoria == null)
                {
                    return NotFound($"Categoria não encontrada {id}.");
                }

                this._appDbContext.SaveChanges();

                return categoria;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar excluir a categoria {id}.");
            }

            
        }
    }
}
