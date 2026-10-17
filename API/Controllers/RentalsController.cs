using API.Dtos;
using API.Mappings;
using Core.Entities;
using Core.Rules;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalsController(StoreContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<RentalDto>> CreateRental(CreateRentalDto dto)
    {
        var game = await context.Games.FindAsync(dto.GameId);
        if (game == null) return NotFound();

        var account = await context.PsnAccounts
            .Where(a => a.GameId == game.Id)
            .Where(AccountRules.HasFreeSlot(dto.Type))
            .FirstOrDefaultAsync();

        if (account == null) return Problem("Нет свободных аккаунтов", statusCode: 400);

        var price = dto.Type == RentalType.WithActivation
            ? game.PriceWithActivation : game.PriceWithoutActivation;

        var rental = new Rental
        {
            PsnAccountId = account.Id,
            Type = dto.Type,
            Status = RentalStatus.Active,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(10),
            Price = price
        };

        context.Rentals.Add(rental);
        await context.SaveChangesAsync();

        var rentalDto = await context.Rentals
            .Where(r => r.Id == rental.Id)
            .Select(RentalMappings.ToDto)
            .FirstAsync();

        return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, rentalDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RentalDto>> GetRental(int id)
    {
        var rental = await context.Rentals
            .Where(r => r.Id == id)
            .Select(RentalMappings.ToDto)
            .FirstOrDefaultAsync();

        if (rental == null) return NotFound();

        return rental;
    }

    [HttpPut("{id:int}/complete")]
    public async Task<ActionResult> CompleteRental(int id)
    {
        var rental = await context.Rentals.FindAsync(id);

        if (rental == null) return NotFound();

        if (rental.Status != RentalStatus.Active) 
            return Problem("Аренда уже завершена", statusCode: 400);
        
        rental.Status = RentalStatus.Completed;

        await context.SaveChangesAsync();

        return NoContent();
    }
}
