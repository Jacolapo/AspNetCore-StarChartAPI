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
        [HttpGet("{id:int}",Name = "GetById")]
        
        public IActionResult GetById(int id)
        {
            var item = _context.CelestialObjects.Find(id);
            if (item == null) return NotFound();

            item.Satellites = _context.CelestialObjects.Where(y => y.OrbitedObjectId == id).ToList();

            return Ok(item);

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var items = _context.CelestialObjects.Where(x => x.Name == name);
            if (!items.Any()) return NotFound();
            foreach (var item in items)
            {
                item.Satellites = _context.CelestialObjects.Where(y => y.OrbitedObjectId == item.Id).ToList();
                
            }
            return Ok(items.ToList());

        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _context.CelestialObjects.ToList();
            foreach (var item in items)
            {
                item.Satellites = _context.CelestialObjects.Where(y => y.OrbitedObjectId == item.Id).ToList();
               
            }
            return Ok(items);
        }


    }
}
