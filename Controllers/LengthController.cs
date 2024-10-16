using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ConversionApplication.URLRequests;

namespace ConversionApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LengthController : Controller
    {
        private readonly AppDbContext context;

        public LengthController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: api/Length
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Length>>> GetAllValues()
        {
            return await context.Length.ToListAsync();
        }

        // GET: api/Length/id{id}
        [HttpGet("id/{id}")]
        public IActionResult GetValueById(int id)
        {
            var value = context.Length.Find(id);
            if (value == null) return NotFound();
            return Ok(value);
        }

        // GET: api/Length/value{value}
        [HttpGet("value/{value}")]
        public async Task<ActionResult<IEnumerable<Length>>> SearchConversions(double value)
        {
            var conversions = await context.Length
                .Where(c =>
                    c.Millimeters == value ||
                    c.Meters == value ||
                    c.Inches == value ||
                    c.Feet == value ||
                    c.Yards == value ||
                    c.Miles == value)
                .ToListAsync();

            if (!conversions.Any())
            {
                return NotFound($"No units found for the value: {value}");
            }

            return Ok(conversions);
        }

        // POST: api/Length
        [HttpPost]
        public async Task<ActionResult<Length>> ConvertLength([FromBody] LengthRequest request)
        {
            var conversion = new Length { };

            // Виконання конвертації
            switch (request.unit.ToLower())
            {
                case "millimeter":
                case "mm":
                    conversion.Miles = request.value / 1609340;
                    conversion.Meters = request.value / 1000;
                    conversion.Feet = request.value / 304.8;
                    conversion.Yards = request.value / 914.4;
                    conversion.Inches = request.value / 25.4;
                    conversion.Millimeters = request.value;
                    break;

                case "meter":
                case "m":
                    conversion.Miles = request.value / 1609.34;
                    conversion.Feet = request.value * 3.28084;
                    conversion.Yards = request.value * 1.09361;
                    conversion.Millimeters = request.value * 1000;
                    conversion.Inches = request.value * 39.3701;
                    conversion.Meters = request.value;
                    break;

                case "inch":
                    conversion.Miles = request.value / 63360;
                    conversion.Meters = request.value / 39.3701;
                    conversion.Feet = request.value / 12;
                    conversion.Yards = request.value / 36;
                    conversion.Millimeters = request.value * 25.4;
                    conversion.Inches = request.value;
                    break;

                case "foot":
                    conversion.Miles = request.value / 5280;
                    conversion.Meters = request.value / 3.28084;
                    conversion.Yards = request.value / 3;
                    conversion.Millimeters = request.value * 304.8;
                    conversion.Inches = request.value * 12;
                    conversion.Feet = request.value;
                    break;

                case "yard":
                    conversion.Miles = request.value / 1760;
                    conversion.Meters = request.value / 1.09361;
                    conversion.Feet = request.value * 3;
                    conversion.Millimeters = request.value * 914.4;
                    conversion.Inches = request.value * 36;
                    conversion.Yards = request.value;
                    break;

                case "mile":
                    conversion.Meters = request.value * 1609.34;
                    conversion.Feet = request.value * 5280;
                    conversion.Yards = request.value * 1760;
                    conversion.Millimeters = request.value * 1609340;
                    conversion.Inches = request.value * 63360;
                    conversion.Miles = request.value;
                    break;
            }

            // Збереження результату в базі даних
            context.Length.Add(conversion);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllValues), new { id = conversion.Id }, conversion);
        }

        // PUT: api/Length{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Length>> UpdateValue(int id, [FromBody] Length value)
        {
            if (id != value.Id) return BadRequest();

            context.Entry(value).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Length
        [HttpDelete]
        public async Task<IActionResult> DeleteAllValues()
        {
            var allValues = await context.Length.ToListAsync();

            if (!allValues.Any()) return NotFound("No values to delete.");

            context.Length.RemoveRange(allValues);
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name = 'Length';");

            return NoContent();
        }
        // DELETE: api/Length{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Length>> DeleteValue(int id)
        {
            var value = await context.Length.FindAsync(id);
            if (value == null) return NotFound();

            context.Length.Remove(value);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}