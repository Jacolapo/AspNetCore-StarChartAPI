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
        [HttpGet("{id:int}", Name = "GetById")]

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
        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = item.Id }, item);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject item)
        {
            var search = _context.CelestialObjects.Find(id);
            if (search == null) return NotFound();
            search.Name = item.Name;
            search.OrbitalPeriod = item.OrbitalPeriod;
            search.OrbitedObjectId = item.OrbitedObjectId;
            _context.Update(search);
            _context.SaveChanges();
            return NoContent();

        }
        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var search = _context.CelestialObjects.Find(id);
            if (search == null) return NotFound();
            search.Name = name;
            _context.Update(search);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var items = _context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id).ToList();
            if (items.Count == 0) return NotFound();
            _context.RemoveRange(items);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
