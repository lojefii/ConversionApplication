using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConversionApplication.URLRequests;

namespace ConversionApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly AppDbContext context;

        public TemperatureController(AppDbContext context)
        {
            context = context;
        }

        // GET: api/Temperature
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Temperature>>> GetAllConversions()
        {
            return await context.Temperature.ToListAsync();
        }

        // GET: api/Temperature/id{id}
        [HttpGet("id/{id}")]
        public IActionResult GetConversionById(int id)
        {
            Temperature? conversion;
            try
            {
                conversion = context.Temperature.Find(id);
            }
            catch (Microsoft.Data.Sqlite.SqliteException ex)
            {
                return NotFound();
            }
            return Ok(conversion); // Повертає знайдений об'єкт
        }

        // GET: api/Temperature/value{value}
        [HttpGet("value/{value}")]
        public async Task<ActionResult<IEnumerable<Temperature>>> SearchConversions(double value)
        {
            var conversions = await context.Temperature
                .Where(c => c.Celsius == value ||
                    c.Fahrenheit == value ||
                    c.Kelvin == value ||
                    c.Reomur == value)
                .ToListAsync();

            if (!conversions.Any())
            {
                return NotFound($"No units found for the value: {value}");
            }

            return Ok(conversions);
        }

        // POST: api/Temperature
        [HttpPost]
        public async Task<ActionResult<Temperature>> ConvertTemperature([FromBody] TemperatureRequest request)
        {
            double fahrenheit = (request.celsius * 9 / 5) + 32;
            double kelvin = (request.celsius + 273.15);
            double reomur = request.celsius * 0.8;
            var conversion = new Temperature
            {
                Celsius = request.celsius,
                Fahrenheit = fahrenheit,
                Kelvin = kelvin,
                Reomur = reomur,
                Date = DateTime.Now
            };

            context.Temperature.Add(conversion);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllConversions), new { id = conversion.Id }, conversion);
        }

        // PUT: api/Temperature{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Temperature>> UpdateValue(int id, [FromBody] Temperature value)
        {
            if (id != value.Id) return BadRequest();

            context.Entry(value).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Temperature
        [HttpDelete]
        public async Task<IActionResult> DeleteAllValues()
        {
            var allValues = await context.Temperature.ToListAsync();

            if (!allValues.Any()) return NotFound("No values to delete.");

            context.Temperature.RemoveRange(allValues);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name = 'Temperature';");

            return NoContent();
        }

        // DELETE: api/Temperature{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValue(int id)
        {
            var value = await context.Temperature.FindAsync(id);
            if (value == null) return NotFound();

            context.Temperature.Remove(value);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}