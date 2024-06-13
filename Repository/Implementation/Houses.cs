using Microsoft.EntityFrameworkCore;

public class Houses : IHouses
{
    // Declara o context
    private readonly GameOfThronesContext _context;

    public Houses(GameOfThronesContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<House>> GetAllAsync()
    {
        return await _context.Houses.ToListAsync();
    }
    public async Task<House> GetByIdAsync(int id)
    {
        // Busca pela House com o id desejado e verifica se é null ou não.
        var house = await _context.Houses.FindAsync(id);
        if (house == null)
        {
            throw new ArgumentException($"House with id {id} not found!");
        }
        return house;
    }
    public async Task AddAsync(House house)
    {
        // Cria uma House
        _context.Houses.Add(house);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(House house)
    {
        #region Explanation
        // _context.Entry(): O método Entry obtém um objeto EntityEntry que fornece acesso a informações sobre a entidade house e suas operações de rastreamento dentro do contexto. Em outras palavras, ele permite que você veja e manipule o estado da entidade house no contexto.
        // .State: representa o estado atual da entidade house no contexto. O estado indica o que o Entity Framework (EF) deve fazer com a entidade quando SaveChanges for chamado.
        // EntityState: É uma enumeração que define os diferentes estados que uma entidade pode ter no contexto.
        //      Detached: A entidade não está sendo rastreada pelo contexto.
        //      Unchanged: A entidade está sendo rastreada pelo contexto, mas não sofreu nenhuma modificação desde que foi carregada ou anexada.
        //      Added: A entidade foi adicionada ao contexto e será inserida no banco de dados quando SaveChanges for chamado.
        //      Deleted: A entidade foi marcada para ser excluída do banco de dados.
        //      Modified: Alguma propriedade da entidade foi modificada e suas mudanças serão atualizadas no banco de dados quando SaveChanges for chamado.
        // EntityState.Modified:
        // Modified: Especifica que a entidade foi modificada. Quando o estado de uma entidade é definido como Modified, o EF entende que deve atualizar todas as propriedades da entidade no banco de dados durante a próxima operação SaveChanges. Isso é útil quando você deseja garantir que as alterações feitas na entidade sejam persistidas.
        #endregion
        _context.Entry(house).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var house = await _context.Houses.FindAsync(id);
        if (house != null)
        {
            _context.Houses.Remove(house);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<bool> HasHousesAsync(int id)
    {
        return await _context.Houses.AnyAsync(h => h.HouseId == id);
    }
}