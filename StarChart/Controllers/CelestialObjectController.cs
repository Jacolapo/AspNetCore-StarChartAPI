using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}")]
        
        public IActionResult GetById(int id)
        {
            var item = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();

            if(item.Id == item.OrbitedObjectId)
            {
                item.Satellites.Add(item);

            }

            return Ok(item.Id);

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var items = _context.CelestialObjects.Where(x => x.Name == name);
            if (items == null) return NotFound();
            foreach (var item in items)
            {
                item.Satellites.Add(item);
            }
            return Ok(items);

        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _context.CelestialObjects;
            foreach (var item in items)
            {
                item.Satellites.Add(item);
            }
            return Ok(items);
        }


    }
}
