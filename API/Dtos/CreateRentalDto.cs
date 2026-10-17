using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace API.Dtos;

public class CreateRentalDto
{
    public int GameId { get; set; }

    [EnumDataType(typeof(RentalType))]
    public RentalType Type { get; set; }
}
