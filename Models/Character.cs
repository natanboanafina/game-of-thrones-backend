using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

public class Character
{
    public int CharacterId { get; set; }

    public int DataId { get; set; }

    [ForeignKey("DataId")]
    public required virtual Data Data { get; set; }

}