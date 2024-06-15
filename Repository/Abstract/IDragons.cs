public interface IDragons
{
    Task<IEnumerable<Dragon>> GetAllAsync();
    Task<Dragon> GetByIdAsync(int id);
    Task AddAsync(Dragon dragon);
    Task UpdateAsync(Dragon dragon);
    Task DeleteAsync(int id);
    Task<bool> HasDragonAsync(int id);
}