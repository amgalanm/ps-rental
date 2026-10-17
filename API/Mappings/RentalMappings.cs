using System.Linq.Expressions;
using API.Dtos;
using Core.Entities;

namespace API.Mappings;

public static class RentalMappings
{
    public static Expression<Func<Rental, RentalDto>> ToDto => r => new RentalDto
    {
        Id = r.Id,
        GameName = r.PsnAccount.Game.Name,
        Type = r.Type,
        Status = r.Status,
        StartDate = r.StartDate,
        EndDate = r.EndDate,
        Price = r.Price
    };
}
