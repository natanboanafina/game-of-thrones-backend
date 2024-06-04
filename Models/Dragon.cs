using System.ComponentModel.DataAnnotations.Schema;

public class Dragon
{

    public int DragonId { get; set; }

    public int DataId { get; set; }

    [ForeignKey("DataId")]
    public required virtual Data Data { get; set; }
    public required string Owner { get; set; }
}