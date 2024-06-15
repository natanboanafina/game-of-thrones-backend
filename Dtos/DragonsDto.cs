public class DragonsDto
{

    public int DragonId { get; set; }
    public int DataId { get; set; }
    public required Data Data { get; set; }
    public required string Owner { get; set; }
}