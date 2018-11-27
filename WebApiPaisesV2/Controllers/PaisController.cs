using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPaisesV2.Models;

namespace WebApiPaisesV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class PaisController : ControllerBase
    {
        private readonly ApplicationDbContext context;


        public PaisController(ApplicationDbContext context) {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Pais> Get() {

            return context.Paises.ToList();
        }

        [HttpGet("{id}", Name ="paisCreado")]
        public IActionResult GetById(int id)
        {
            var pais = context.Paises.Include(x => x.Provincias).FirstOrDefault(x => x.Id == id);
            if (pais == null)
            {
                return NotFound();
            }
            return Ok(pais);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Pais pais) {
            if (ModelState.IsValid) {
                context.Paises.Add(pais);
                context.SaveChanges();
                return new CreatedAtRouteResult("paisCreado", new { id = pais.Id }, pais);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Pais pais, int id)
        {
            if (pais.Id!= id)
            {
                return BadRequest(ModelState);
               
            }

            context.Entry(pais).State=EntityState.Modified ;
            context.SaveChanges();
            return Ok();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pais = context.Paises.FirstOrDefault(x => x.Id == id);
            if (pais == null)
            {
                return BadRequest(ModelState);
            }
            context.Paises.Remove(pais);
            context.SaveChanges();
            return Ok();

        }




    }
}