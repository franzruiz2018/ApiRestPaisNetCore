using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPaisesV2.Models;

namespace WebApiPaisesV2.Controllers
{
    [Produces("application/json")]
    [Route("api/Pais/{PaisId}/Provincia")]  
    public class ProvinciaController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProvinciaController(ApplicationDbContext context) {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Provincia> GetAll(int PaisId)
        {

            return context.Provincias.Where(x => x.PaisId == PaisId).ToList() ;

        }

        [HttpGet("{id}", Name = "provinciaCreado")]
        public IActionResult GetById(int id)
        {
            var provincia = context.Provincias.FirstOrDefault(x => x.Id == id);
            if (provincia == null)
            {
                return NotFound();
            }
            return Ok(provincia);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Provincia provincia, int paisId)
        {
            provincia.PaisId = paisId;
            if (ModelState.IsValid)
            {
                context.Provincias.Add(provincia);
                context.SaveChanges();
                return new CreatedAtRouteResult("paisCreado", new { id = provincia.Id }, provincia);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Provincia provincia, int id)
        {
            if (provincia.Id != id)
            {
                return BadRequest(ModelState);

            }

            context.Entry(provincia).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var provincia = context.Provincias.FirstOrDefault(x => x.Id == id);
            if (provincia == null)
            {
                return BadRequest(ModelState);
            }
            context.Provincias.Remove(provincia);
            context.SaveChanges();
            return Ok();

        }




    }
}