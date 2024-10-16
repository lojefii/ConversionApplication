using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConversionApplication.URLRequests;

namespace ConversionApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyController : Controller
    {

        private readonly AppDbContext context;

        public EnergyController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: api/Energy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Energy>>> GetAllConversions()
        {
            return await context.Energy.ToListAsync();
        }

        // GET: api/Energy/id/{id}
        [HttpGet("id/{id}")]
        public IActionResult GetConversionById(int id)
        {
            Energy? conversion;
            try
            {
                conversion = context.Energy.Find(id);
            }
            catch (Microsoft.Data.Sqlite.SqliteException ex)
            {
                return NotFound();
            }
            return Ok(conversion);
        }

        // GET: api/Energy/value/{value}
        [HttpGet("value/{value}")]
        public async Task<ActionResult<IEnumerable<Energy>>> SearchConversions(double value)
        {
            var conversions = await context.Energy
                .Where(c => c.Joules == value ||
                    c.Calories == value ||
                    c.KilowattHours == value ||
                    c.BTU == value)
                .ToListAsync();

            if (!conversions.Any())
            {
                return NotFound($"No units found for the value: {value}");
            }

            return Ok(conversions);
        }

        // POST: api/Energy
        [HttpPost]
        public async Task<ActionResult<Energy>> ConvertEnergy([FromBody] EnergyRequest request)
        {
            double calories = request.joules / 4.184;
            double kilowattHours = request.joules / 3.6e6;
            double BTU = request.joules / 1055.06;
            var conversion = new Energy
            {
                Calories = calories,
                KilowattHours = kilowattHours,
                BTU = BTU,
                Joules = request.joules,
                Date = DateTime.Now
            };

            context.Energy.Add(conversion);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllConversions), new { id = conversion.Id }, conversion);
        }

        // PUT: api/Energy{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Energy>> UpdateValue(int id, [FromBody] Energy value)
        {
            if (id != value.Id) return BadRequest();

            context.Entry(value).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Energy
        [HttpDelete]
        public async Task<IActionResult> DeleteAllValues()
        {
            var allValues = await context.Energy.ToListAsync();

            if (!allValues.Any()) return NotFound("No values to delete.");

            context.Energy.RemoveRange(allValues);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name = 'Energy';");

            return NoContent();
        }

        // DELETE: api/Energy{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValue(int id)
        {
            var value = await context.Energy.FindAsync(id);
            if (value == null) return NotFound();

            context.Energy.Remove(value);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
