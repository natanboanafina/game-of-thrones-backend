using System.ComponentModel.DataAnnotations.Schema;

public class House
{
    public int HouseId { get; set; }
    public required string HouseName { get; set; }

    public int DataId { get; set; }

    [ForeignKey("DataId")]
    public required virtual Data Data { get; set; }

}