using System.Security.Claims;
using API.Dtos;
using API.Mappings;
using Core.Entities;
using Core.Rules;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalsController(StoreContext context) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<RentalDto>> CreateRental(CreateRentalDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var game = await context.Games.FindAsync(dto.GameId);
        if (game == null || game.IsArchived) return NotFound();

        var noFreeSlotMessage = $"Нет свободных аккаунтов для {game.Name} ({dto.Type})";

        var account = await context.PsnAccounts
            .Where(a => a.GameId == game.Id)
            .Where(AccountRules.HasFreeSlot(dto.Type))
            .FirstOrDefaultAsync();

        if (account == null) return Problem(noFreeSlotMessage, statusCode: 409);

        var price = dto.Type == RentalType.WithActivation
            ? game.PriceWithActivation : game.PriceWithoutActivation;

        var rental = new Rental
        {
            PsnAccountId = account.Id,
            Type = dto.Type,
            Status = RentalStatus.Active,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(10),
            Price = price,
            AppUserId = userId
        };

        context.Rentals.Add(rental);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation())
        {
            return Problem(noFreeSlotMessage, statusCode: 409);
        }

        var rentalDto = await context.Rentals
            .Where(r => r.Id == rental.Id)
            .Select(RentalMappings.ToDto)
            .FirstAsync();

        return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, rentalDto);
    }

    [HttpGet("{id:int}/credentials")]
    [Authorize]
    public async Task<ActionResult<PsnCredentialsDto>> GetCredentials(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var rental = await context.Rentals
            .Include(r => r.PsnAccount)
                .ThenInclude(a => a.Game)
            .Where(r => r.AppUserId == userId)
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (rental == null) return NotFound();

        if (rental.Status != RentalStatus.Active)
            return Problem("Срок аренды завершен", statusCode: 409);

        return new PsnCredentialsDto
        {
            GameName = rental.PsnAccount.Game.Name,
            Type = rental.Type,
            Email = rental.PsnAccount.Email,
            Password = rental.PsnAccount.Password,
            EndDate = rental.EndDate
        };
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<RentalDto>> GetRental(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var rental = await context.Rentals
            .Where(r => r.AppUserId == userId)
            .Where(r => r.Id == id)
            .Select(RentalMappings.ToDto)
            .FirstOrDefaultAsync();

        if (rental == null) return NotFound();

        return rental;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<RentalDto>>> GetRentals()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return await context.Rentals
            .Where(r => r.AppUserId == userId)
            .Select(RentalMappings.ToDto)
            .ToListAsync();
    }

    [HttpPut("{id:int}/complete")]
    [Authorize(Roles = "Admin")]
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
