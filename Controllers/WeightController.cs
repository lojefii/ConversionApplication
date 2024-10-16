using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConversionApplication.URLRequests;

namespace ConversionApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightController : Controller
    {

        private readonly AppDbContext context;

        public WeightController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: api/Weight
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Weight>>> GetAllConversions()
        {
            return await context.Weight.ToListAsync();
        }

        // GET: api/Weight/id{id}
        [HttpGet("id/{id}")]
        public IActionResult GetConversionById(int id)
        {
            Weight? conversion;
            try
            {
                conversion = context.Weight.Find(id);
            }
            catch (Microsoft.Data.Sqlite.SqliteException ex)
            {
                return NotFound();
            }
            return Ok(conversion);
        }

        // GET: api/Weight/value{value}
        [HttpGet("value/{value}")]
        public async Task<ActionResult<IEnumerable<Weight>>> SearchConversions(double value)
        {
            var conversions = await context.Weight
                .Where(c => c.Grams == value ||
                    c.Ounces == value ||
                    c.Karats == value ||
                    c.Pounds == value)
                .ToListAsync();

            if (!conversions.Any())
            {
                return NotFound($"No units found for the value: {value}");
            }

            return Ok(conversions);
        }

        // POST: api/Weight
        [HttpPost]
        public async Task<ActionResult<Weight>> ConvertWeight([FromBody] WeightRequest request)
        {
            double karat = request.grams * 5;
            double pound = request.grams / 453.58237;
            double ounce = request.grams / 28.3495;
            var conversion = new Weight
            {
                Ounces = ounce,
                Pounds = pound,
                Karats = karat,
                Grams = request.grams,
                Date = DateTime.Now
            };

            context.Weight.Add(conversion);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllConversions), new { id = conversion.Id }, conversion);
        }

        // PUT: api/Weight{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Weight>> UpdateValue(int id, [FromBody] Weight value)
        {
            if (id != value.Id) return BadRequest();

            context.Entry(value).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Weight
        [HttpDelete]
        public async Task<IActionResult> DeleteAllValues()
        {
            var allValues = await context.Weight.ToListAsync();

            if (!allValues.Any()) return NotFound("No values to delete.");

            context.Weight.RemoveRange(allValues);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name = 'Weight';");

            return NoContent();
        }

        // DELETE: api/Weight{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValue(int id)
        {
            var value = await context.Weight.FindAsync(id);
            if (value == null) return NotFound();

            context.Weight.Remove(value);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
