using Microsoft.EntityFrameworkCore;

public class Characters : ICharacters
{
    private readonly GameOfThronesContext _context;

    public Characters(GameOfThronesContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Character>> GetAllAsync()
    {
        return await _context.Characters.ToListAsync();
    }

    public async Task<Character> GetByIdAsync(int id)
    {
        var character = await _context.Characters.FindAsync(id);
        if (character == null)
        {
            throw new ArgumentException($"Character with id {id} not found!");
        }
        return character;
    }

    public async Task AddAsync(Character character)
    {
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Character character)
    {
        _context.Entry(character).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var character = await _context.Characters.FindAsync(id);
        if (character != null)
        {
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasCharactersAsync(int id)
    {
        return await _context.Characters.AnyAsync(e => e.CharacterId == id);
    }
}