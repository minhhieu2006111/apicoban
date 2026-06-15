using apicoban.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace apicoban.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> _products = new List<Product>();
        private static int _nextId = 1;

        [HttpPost]
        public IActionResult Create([FromBody] Product model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            model.Id = _nextId++;
            _products.Add(model);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0) return BadRequest(new[] { "Id must be a positive integer." });
            var p = _products.FirstOrDefault(x => x.Id == id);
            if (p == null) return NotFound();
            return Ok(p);
        }
    }
}
