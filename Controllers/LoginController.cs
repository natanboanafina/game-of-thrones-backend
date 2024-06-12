// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;


// [ApiController]
// [Route("api/[controller]")]
// public class LoginsController : ControllerBase
// {
//     private readonly GameOfThronesContext _context;

//     public LoginsController(GameOfThronesContext context) => _context = context;

//     // GET
//     [HttpGet(Name = "GetCharacters")]
//     public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
//     {
//         var characters = await _context.Characters
//         .Include(c => c.Data)
//         .ToListAsync();

//         return characters;
//     }

//     [HttpGet("{id}", Name = "GetCharacterById")]
//     public async Task<ActionResult<Character>> GetCharacter(int id)
//     {
//         var character = await _context.Characters.Include(c => c.Data).FirstOrDefaultAsync(c => c.CharacterId == id);

//         if (character == null)
//         {
//             return NotFound();
//         }

//         return Ok(character);
//     }

//     [HttpPost]
//     public async Task<ActionResult<Character>> PostCharacter(Character character)
//     {
//         _context.Characters.Add(character);
//         await _context.SaveChangesAsync();
//         return Ok(await _context.Characters.Include(c => c.Data).ToListAsync());
//     }

//     [HttpPut("{id}")]
//     public async Task<IActionResult> PutCharacter(int id, Character character)
//     {
//         if (id != character.CharacterId)
//         {
//             return BadRequest();
//         }

//         var hasCharacter = await _context.Characters.Include(c => c.Data).FirstOrDefaultAsync(c => c.CharacterId == id);
//         if (hasCharacter == null)
//         {
//             return NotFound();
//         }

//         hasCharacter.Data.Name = character.Data.Name;
//         hasCharacter.Data.Image = character.Data.Image;
//         hasCharacter.Data.Description = character.Data.Description;
//         hasCharacter.Titles = character.Titles;
//         hasCharacter.Gender = character.Gender;
//         hasCharacter.House = character.House;
//         hasCharacter.Culture = character.Culture;
//         hasCharacter.Born = character.Born;

//         try
//         {
//             await _context.SaveChangesAsync();
//         }
//         catch (DbUpdateConcurrencyException)
//         {
//             if (!CharacterExists(id))
//             {
//                 return NotFound();
//             }
//             else
//             {
//                 throw new DbUpdateConcurrencyException("Ocorreu um erro de concorrência durante a atualização do cliente.");
//             }
//         }
//         return Ok(hasCharacter);
//     }

//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteCharacter(int id)
//     {
//         var character = await _context.Characters.FindAsync(id);
//         if (character == null)
//         {
//             return NotFound();
//         }

//         _context.Characters.Remove(character);
//         await _context.SaveChangesAsync();
//         return Ok($"Personagem de id {id} deletado!");
//     }
//     private bool CharacterExists(int id)
//     {
//         return _context.Characters.Any(e => e.CharacterId == id);
//     }
// }
