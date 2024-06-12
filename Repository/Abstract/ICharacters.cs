public interface ICharacters
{
    Task<IEnumerable<Character>> GetAllAsync();
    Task<Character> GetByIdAsync(int id);
    Task AddAsync(Character character);
    Task UpdateAsync(Character character);
    Task DeleteAsync(int id);
    Task<bool> HasCharactersAsync(int id);
}