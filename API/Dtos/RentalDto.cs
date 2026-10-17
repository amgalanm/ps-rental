using Core.Entities;

namespace API.Dtos;

public class RentalDto
{
    public int Id { get; set; }
    public string GameName { get; set; } = string.Empty;
    public RentalType Type { get; set; }
    public RentalStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
}
