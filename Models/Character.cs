using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

public class Character
{
    public int CharacterId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

}