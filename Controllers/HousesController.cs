using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class HousesController : ControllerBase
{

    private readonly IHouses _housesRepository;
    private readonly IFileService _fileService;
    private readonly IWebHostEnvironment _environment;

    public HousesController(IHouses housesRepository, IFileService fileService, IWebHostEnvironment environment)
    {
        _housesRepository = housesRepository;
        _fileService = fileService;
        _environment = environment;
    }

    [HttpGet(Name = "GetHouses")]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult<IEnumerable<House>>> GetHouses()
    {
        var houses = await _housesRepository.GetAllAsync();
        var housesDto = houses.Select(h => new HousesDto
        {
            HouseId = h.HouseId,
            HouseName = h.HouseName,
            Data = new DataDto
            {
                DataId = h.Data.DataId,
                Name = h.Data.Name,
                Description = h.Data.Description,
                Image = h.Data.Image,
            },
            Lord = h.Lord,
            Region = h.Region,

        });
        // var houses = await _context.Houses
        // .Include(h => h.Data)
        // .ToListAsync();

        return Ok(housesDto);
    }

    [HttpGet("{id}", Name = "GetHouseById")]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult<House>> GetHouse(int id)
    {
        var house = await _housesRepository.GetByIdAsync(id);
        if (house == null)
        {
            return NotFound();
        }

        var houseDto = new HousesDto
        {
            HouseId = house.HouseId,
            HouseName = house.HouseName,
            Data = new DataDto
            {
                DataId = house.Data.DataId,
                Name = house.Data.Name,
                Image = house.Data.Image,
                Description = house.Data.Description,
            },
            Lord = house.Lord,
            Region = house.Region,
        };
        return Ok(houseDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<House>> PostHouse([FromForm] HousesDto houseDto, IFormFile imageFile)
    {
        var result = await _fileService.SaveImageAsync(imageFile, "houses");
        if (result.Item1 == 0)
        {
            return BadRequest(result.Item2);
        }

        var house = new House
        {
            HouseId = houseDto.HouseId,
            HouseName = houseDto.HouseName,
            Data = new Data
            {
                DataId = houseDto.Data.DataId,
                Name = houseDto.Data.Name,
                Image = result.Item2,
                Description = houseDto.Data.Description,
            },
            Lord = houseDto.Lord,
            Region = houseDto.Region,
        };

        await _housesRepository.AddAsync(house);

        return CreatedAtAction(
            "GetHouseById",
            new { id = house.HouseId },
            houseDto
            );
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutHouse(int id, [FromForm] HousesDto houseDto, IFormFile imageFile)
    {
        if (id != houseDto.HouseId)
        {
            return BadRequest();
        }

        var house = await _housesRepository.GetByIdAsync(id);

        if (house == null)
        {
            return NotFound();
        }

        if (imageFile != null)
        {
            await _fileService.DeleteImageAsync(house.Data.Image, "houses");
            var result = await _fileService.SaveImageAsync(imageFile, "houses");

            if (result.Item1 == 0)
            {
                return BadRequest(result.Item2);
            }

            house.Data.Image = result.Item2;
        }

        house.HouseId = houseDto.DataId;
        house.Data.Name = houseDto.Data.Name;
        house.Data.Description = houseDto.Data.Description;
        house.Lord = houseDto.Lord;
        house.Region = houseDto.Region;

        try
        {
            await _housesRepository.UpdateAsync(house);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _housesRepository.HasHousesAsync(id))
            {
                return NotFound();
            }
            else
            {
                throw new DbUpdateConcurrencyException("Ocorreu um erro de concorrência durante a atualização do cliente.");
            }
        }
        return Ok(houseDto); // Retorna os dados atualizados
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteHouse(int id)
    {
        var house = await _housesRepository.GetByIdAsync(id);
        if (house == null)
        {
            return NotFound();
        }

        await _fileService.DeleteImageAsync(house.Data.Image, "houses");
        await _housesRepository.DeleteAsync(id);
        return Ok($"Casa de id{id} deletada!");
    }
}