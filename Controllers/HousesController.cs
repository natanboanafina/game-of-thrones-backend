using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class HousesController : Controller
{
    private readonly GameOfThronesContext _context;

    public HousesController(GameOfThronesContext context) => _context = context;

    [HttpGet(Name = "GetHouses")]
    public async Task<ActionResult<IEnumerable<House>>> GetHouses()
    {
        var houses = await _context.Houses
        .Include(h => h.Data)
        .ToListAsync();

        return houses;
    }

    [HttpGet("{id}", Name = "GetHouseById")]
    public async Task<ActionResult<House>> GetHouse(int id)
    {
        var house = await _context.Houses.Include(h => h.HouseId).FirstOrDefaultAsync(h => h.HouseId == id);

        if (house == null)
        {
            return NotFound();
        }
        return Ok(house);
    }

    [HttpPost]
    public async Task<ActionResult<House>> PostHouse(House house)
    {
        _context.Houses.Add(house);
        await _context.SaveChangesAsync();

        return Ok(await _context.Houses.Include(h => h.HouseId).ToListAsync());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHouse(int id, House house)
    {
        if (id != house.HouseId)
        {
            return BadRequest();
        }

        var existingHouse = await _context.Houses.Include(h => h.Data).FirstOrDefaultAsync(h => h.HouseId == id);
        if (existingHouse == null)
        {
            return NotFound();
        }

        existingHouse.Data.Name = house.Data.Name;
        existingHouse.Data.Description = house.Data.Description;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!HouseExists(id))
            {
                return NotFound();
            }
            else
            {
                throw new DbUpdateConcurrencyException("Ocorreu um erro de concorrência durante a atualização do cliente.");
            }
        }
        return Ok(existingHouse); // Retorna os dados atualizados
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHouse(int id)
    {
        var house = await _context.Houses.FindAsync(id);
        if (house == null)
        {
            return NotFound();
        }
        _context.Houses.Remove(house);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool HouseExists(int id)
    {
        return _context.Houses.Any(e => e.HouseId == id);
    }
}