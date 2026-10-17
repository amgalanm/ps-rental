using API.Dtos;
using API.Mappings;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController(StoreContext context) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameDto>> GetGame(int id)
    {
        var game = await context.Games
            .Where(g => g.Id == id)
            .Select(GameMappings.ToDto)
            .FirstOrDefaultAsync();

        if(game == null) return NotFound();
        
        return game;
    }

    [HttpGet]
    public async Task<ActionResult<List<GameDto>>> GetGames()
    {
        return await context.Games
            .Select(GameMappings.ToDto)
            .ToListAsync();
    }
}
