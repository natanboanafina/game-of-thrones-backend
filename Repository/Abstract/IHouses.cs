public interface IHouses
{
    Task<IEnumerable<House>> GetAllAsync();
    Task<House> GetByIdAsync(int id);
    Task AddAsync(House house);
    Task UpdateAsync(House house);
    Task DeleteAsync(int id);
    Task<bool> HasHousesAsync(int id);
}