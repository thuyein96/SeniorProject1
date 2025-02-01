namespace SnowFlake.Dtos.APIs.Product.GetProducts;

public class GetProductsByTeamRequest
{
    public int TeamNumber { get; set; }
    public string? PlayerRoomCode { get; set; }
    public string? HostRoomCode { get; set; }
}