using System.ComponentModel.DataAnnotations.Schema;

public class Character
{
    public int CharacterId { get; set; }

    public int DataId { get; set; }

    [ForeignKey("DataId")]
    public required virtual Data Data { get; set; }
    public required List<string> Titles { get; set; }
    public required string Gender { get; set; }
    public required string House { get; set; }
    public required string Culture { get; set; }
    public required string Born { get; set; }

}