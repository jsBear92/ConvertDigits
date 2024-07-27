using ConvertDigits.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConvertDigits.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class ConvertController : Controller
{
    private readonly NumberContext _context;
    public ConvertController(NumberContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Numbers>>> GetAllNumbers()
    {
        return await _context.Numbers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Numbers>> GetNumber(int id)
    {
        var number = await _context.Numbers.FindAsync(id);
        if (number == null)
        {
            return NotFound();
        }
        return number;
    }

    [HttpPost]
    public async Task<ActionResult<Numbers>> PostNumber(Numbers number)
    {
        _context.Numbers.Add(number);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetNumber), new { id = number.Id }, number);
    }
}