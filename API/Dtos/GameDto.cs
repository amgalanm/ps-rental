using Core.Entities;

namespace API.Dtos;

public class GameDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }
    public GamePlatform Platform { get; set; }
    public decimal PriceWithActivation { get; set; }
    public decimal PriceWithoutActivation { get; set; }
    public bool IsAvailableWithActivation { get; set; }
    public bool IsAvailableWithoutActivation { get; set; }
}
