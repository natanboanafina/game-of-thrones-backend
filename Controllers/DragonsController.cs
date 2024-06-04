
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class DragonsController : Controller
{
    private readonly GameOfThronesContext _context;
    public DragonsController(GameOfThronesContext context) => _context = context;

    [HttpGet("GetDragons")]
    public async Task<ActionResult<IEnumerable<Dragon>>> GetDragons()
    {
        var dragons = await _context.Dragons
        .Include(d => d.Data)
        .ToListAsync();

        return dragons;
    }

    [HttpGet("{id}", Name = "GetDragonById")]
    public async Task<ActionResult<Dragon>> GetDragon(int id)
    {
        var dragon = await _context.Dragons
        .Include(d => d.Data)
        .FirstOrDefaultAsync(d => d.DragonId == id);

        if (dragon == null)
        {
            return NotFound();
        }
        return Ok(dragon);

    }

    [HttpPost]
    public async Task<ActionResult<Dragon>> PostDragon(Dragon dragon)
    {
        _context.Dragons.Add(dragon);
        await _context.SaveChangesAsync();
        return Ok(await _context.Dragons.Include(d => d.Data).ToListAsync());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDragon(int id, Dragon dragon)
    {
        if (id != dragon.DragonId)
        {
            return BadRequest();
        }

        var hasDragon = await _context.Dragons.Include(d => d.Data).FirstOrDefaultAsync(d => d.DragonId == id);

        if (hasDragon == null)
        {
            return NotFound();
        }

        hasDragon.Data.Name = dragon.Data.Name;
        hasDragon.Data.Image = dragon.Data.Image;
        hasDragon.Data.Description = dragon.Data.Description;
        hasDragon.Owner = dragon.Owner;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DragonExists(id))
            {
                return NotFound();
            }
            else
            {
                throw new DbUpdateConcurrencyException("Ocorreu um erro de concorrência durante a atualização do cliente.");
            }
        }
        return Ok(hasDragon);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDragon(int id)
    {
        var dragon = await _context.Dragons.FindAsync(id);
        if (dragon == null)
        {
            return NotFound();
        }
        _context.Dragons.Remove(dragon);
        await _context.SaveChangesAsync();

        return Ok($"Dragão de id {id} deletado!");
    }

    private bool DragonExists(int id)
    {
        return _context.Dragons.Any(e => e.DragonId == id);

    }
}