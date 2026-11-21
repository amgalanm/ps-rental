using Core.Entities;

namespace API.Dtos;

public class PsnCredentialsDto
{
    public string GameName { get; set; } = string.Empty;
    public RentalType Type { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
}
