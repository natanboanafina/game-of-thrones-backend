using Microsoft.EntityFrameworkCore;

public class Dragons : IDragons
{
    // Instanciar o contexto
    private readonly GameOfThronesContext _context;

    // Criar o constructor
    public Dragons(GameOfThronesContext context)
    {
        _context = context;
    }

    // Criar GetAllAsync
    public async Task<IEnumerable<Dragon>> GetAllAsync()
    {
        // Retornar a listagem
        return await _context.Dragons.ToListAsync();
    }

    // Criar GetByIdAsync
    public async Task<Dragon> GetByIdAsync(int id)
    {
        // Buscar pelo dragon via ID
        var dragon = await _context.Dragons.FindAsync(id);

        // Verificar se é nulo
        if (dragon == null)
        {
            // Lançar erro se for nulo
            throw new ArgumentException($"Dragon with id {id} not found!");
        }

        // Retornar o valor
        return dragon;
    }

    // Criar o método AddAsync
    public async Task AddAsync(Dragon dragon)
    {
        // Acessar o context e usar o .Add();
        // Passar o valor recebido como parâmetro para .Add();
        _context.Dragons.Add(dragon);
        // Salvar as mudanças
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Dragon dragon)
    {
        _context.Entry(dragon).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var dragon = await _context.Dragons.FindAsync(id);
        if (dragon != null)
        {
            _context.Dragons.Remove(dragon);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasDragonAsync(int id)
    {
        return await _context.Dragons.AnyAsync(d => d.DragonId == id);
    }
}