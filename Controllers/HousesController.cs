using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class HousesController : Controller
{
    private readonly GameOfThronesContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly IFileService _fileService;

    public HousesController(GameOfThronesContext context, IFileService fileService, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _fileService = fileService;
    }

    [HttpGet(Name = "GetHouses")]
    public async Task<ActionResult<IEnumerable<House>>> GetHouses()
    {
        var houses = await _context.Houses
        .Include(h => h.Data)
        .ToListAsync();

        return Ok(houses);
    }

    [HttpGet("{id}", Name = "GetHouseById")]
    public async Task<ActionResult<House>> GetHouse(int id)
    {
        var house = await _context.Houses
        .Include(h => h.HouseId)
        .FirstOrDefaultAsync(h => h.HouseId == id);

        if (house == null)
        {
            return NotFound();
        }
        return Ok(house);
    }

    [HttpPost]
    public async Task<ActionResult<House>> PostHouse([FromForm] House house, IFormFile imageFile)
    {
        if (imageFile != null)
        {
            // salvar as imagem
            var result = await _fileService.SaveImageAsync(imageFile, "Houses");

            if (result.Item1 == 0)
            {
                // Retornar um erro se a imagem não puder ser salva
                return BadRequest(result.Item2);
            }

            // Definir o nome do arquivo da imagem na entidade Data
            house.Data.Image = result.Item2;
        }

        _context.Houses.Add(house);
        await _context.SaveChangesAsync();

        // return Ok(await _context.Houses.Include(h => h.HouseId).ToListAsync());
        return CreatedAtAction(nameof(GetHouse), new { id = house.HouseId }, house);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHouse(int id, House house)
    {
        if (id != house.HouseId)
        {
            return BadRequest();
        }

        var hasHouse = await _context
        .Houses.Include(h => h.Data)
        .FirstOrDefaultAsync(h => h.HouseId == id);

        if (hasHouse == null)
        {
            return NotFound();
        }

        hasHouse.Data.Name = house.Data.Name;

        if (!string.IsNullOrEmpty(house.Data.Image))
        {
            hasHouse.Data.Image = house.Data.Image;
        }
        hasHouse.Data.Description = house.Data.Description;
        hasHouse.Lord = house.Lord;
        hasHouse.Region = house.Region;

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
        return Ok(hasHouse); // Retorna os dados atualizados
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHouse(int id)
    {
        var house = await _context
        .Houses
        .FindAsync(id);

        if (house == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(house.Data.Image))
        {
            var fileService = new FileService(_environment);
            await fileService.DeleteImageAsync(house.Data.Image, "Houses");
        }

        _context.Houses.Remove(house);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [NonAction]
    private bool HouseExists(int id)
    {
        return _context.Houses.Any(e => e.HouseId == id);
    }

    // [NonAction]
    // private string GetFilePath(string HouseCode)
    // {
    //     return _environment.WebRootPath + "\\Uploads\\House\\" + HouseCode;
    // }
}