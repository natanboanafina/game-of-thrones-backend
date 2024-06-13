public class HouseDto
{
    public int HouseId { get; set; }
    public required string HouseName { get; set; }
    public int DataId { get; set; }
    public required virtual Data Data { get; set; }
    public required string Lord { get; set; }
    public required string Region { get; set; }
}