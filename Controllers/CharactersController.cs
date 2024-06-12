using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/[controller]")]
public class CharactersController : ControllerBase
{

    private readonly ICharacters _charactersRepository;

    public CharactersController(ICharacters charactersRepository)
    {
        _charactersRepository = charactersRepository;
    }

    // GET
    [HttpGet(Name = "GetCharacters")]
    [Authorize(Roles = "User,Admin")] // Apenas usuários autenticados podem acessar
    public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
    {
        var characters = await _charactersRepository.GetAllAsync();
        var charactersDto = characters.Select(c => new CharactersDto
        {
            CharacterId = c.CharacterId,
            DataId = c.DataId,
            Data = new DataDto
            {
                DataId = c.Data.DataId,
                Name = c.Data.Name,
                Image = c.Data.Image,
                Description = c.Data.Description,
            },
            Titles = c.Titles,
            Gender = c.Gender,
            House = c.House,
            Culture = c.Culture,
            Born = c.Born,
        });

        return Ok(charactersDto);
    }

    [HttpGet("{id}", Name = "GetCharacterById")]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult<Character>> GetCharacter(int id)
    {
        var character = await _charactersRepository.GetByIdAsync(id);
        if (character == null)
        {
            return NotFound();
        }
        var characterDto = new CharactersDto
        {
            CharacterId = character.CharacterId,
            DataId = character.DataId,
            Data = new DataDto
            {
                DataId = character.Data.DataId,
                Name = character.Data.Name,
                Image = character.Data.Image,
                Description = character.Data.Description,
            },
            Titles = character.Titles,
            Gender = character.Gender,
            House = character.House,
            Culture = character.Culture,
            Born = character.Born,
        };
        return Ok(characterDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Apenas administradores podem acessar
    public async Task<ActionResult<Character>> PostCharacter(CharactersDto characterDto)
    {
        var character = new Character
        {
            DataId = characterDto.DataId,
            Data = new Data
            {
                DataId = characterDto.Data.DataId,
                Name = characterDto.Data.Name,
                Image = characterDto.Data.Image,
                Description = characterDto.Data.Description
            },
            Titles = characterDto.Titles,
            Gender = characterDto.Gender,
            House = characterDto.House,
            Culture = characterDto.Culture,
            Born = characterDto.Born
        };

        await _charactersRepository.AddAsync(character);

        // retorna o caminho para o recurso criado junto com o próprio recurso
        return CreatedAtAction("GetCharacterById", new { id = character.CharacterId }, characterDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Apenas administradores podem acessar
    public async Task<IActionResult> PutCharacter(int id, CharactersDto characterDto)
    {
        if (id != characterDto.CharacterId)
        {
            return BadRequest();
        }
        var character = await _charactersRepository.GetByIdAsync(id);
        if (character == null)
        {
            return NotFound();
        }

        character.DataId = characterDto.DataId;
        character.Data = new Data
        {
            DataId = characterDto.DataId,
            Name = characterDto.Data.Name,
            Image = characterDto.Data.Image,
            Description = characterDto.Data.Description,
        };
        character.Titles = characterDto.Titles;
        character.Gender = characterDto.Gender;
        character.House = characterDto.House;
        character.Culture = characterDto.Culture;
        character.Born = characterDto.Born;

        try
        {
            await _charactersRepository.UpdateAsync(character);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _charactersRepository.HasCharactersAsync(id))
            {
                return NotFound();
            }
            else
            {
                throw new DbUpdateConcurrencyException("Ocorreu um erro de concorrência durante a atualização do cliente.");
            }
        }
        return Ok(characterDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Apenas administradores podem acessar
    public async Task<IActionResult> DeleteCharacter(int id)
    {
        var character = await _charactersRepository.GetByIdAsync(id);
        if (character == null)
        {
            return NotFound();
        }

        await _charactersRepository.DeleteAsync(id);
        return Ok($"Personagem de id {id} deletado!");
    }

}

