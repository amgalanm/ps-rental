using System.Linq.Expressions;
using API.Dtos;
using Core.Entities;
using Core.Rules;

namespace API.Mappings;

public static class GameMappings
{
    public static Expression<Func<Game, GameDto>> ToDto
    {
        get
        {
            var withActivation = AccountRules.HasFreeSlot(RentalType.WithActivation);
            var withoutActivation = AccountRules.HasFreeSlot(RentalType.WithoutActivation);

            return g => new GameDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                PictureUrl = g.PictureUrl,
                Platform = g.Platform,
                PriceWithActivation = g.PriceWithActivation,
                PriceWithoutActivation = g.PriceWithoutActivation,
                IsAvailableWithActivation = g.PsnAccounts.AsQueryable().Any(withActivation),
                IsAvailableWithoutActivation = g.PsnAccounts.AsQueryable().Any(withoutActivation)
            };
        }
    }
}
