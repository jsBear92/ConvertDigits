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
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Numbers>> PostNumber(Numbers number)
    {
        NumberConverter converter = new();
        Numbers words = converter.CheckString(number.Dollars, number.Cents);
        _context.Numbers.Add(words);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetNumber), new { id = words.Id }, words);
    }
}

internal class NumberConverter
{
    private readonly string[] Units = {
        "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE"
    };
    private readonly string[] IrregularTens = {
        " TEN", " ELEVEN", " TWELVE", " THIRTEEN", " FOURTEEN", " FIFTEEN", " SIXTEEN", " SEVENTEEN", " EIGHTEEN", " NINETEEN"
    };
    private readonly string[] Tens = {
        " TWENTY", " THIRTY", " FORTY", " FIFTY", " SIXTY", " SEVENTY", " EIGHTY", " NINETY"
    };
    private readonly string[] Others = {
        " HUNDRED", " THOUSAND", " MILLION", " BILLION"
    };

    public Numbers CheckString(string dollar, string cents)
    {
        Numbers result = new ();
        
        int integerNumber = int.Parse(dollar);
        int fractionalNumber = cents != "" ? int.Parse(cents) : 0;

        result.Dollars = ConvertIntegersToWords(integerNumber);
        result.Cents = ConvertFloatingPointToWords(fractionalNumber);

        return result;
    }

    public string ConvertIntegersToWords(int number)
    {
        string words = "";

        if (number == 0)
            return Units[0];

        if (number < 0)
            return "MINUS " + ConvertIntegersToWords(Math.Abs(number));

        if ((number / 1000000000) > 0)
        {
            words += ConvertIntegersToWords(number / (int)Magnitude.Billion) + Others[3];
            number %= (int)Magnitude.Billion;
        }
        if ((number / 1000000) > 0)
        {
            words += ConvertIntegersToWords(number / (int)Magnitude.Million) + Others[2];
            number %= (int)Magnitude.Million;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertIntegersToWords(number / (int)Magnitude.Thousand) + Others[1];
            number %= (int)Magnitude.Thousand;
        }
        if ((number / 100) > 0)
        {
            words += ConvertIntegersToWords(number / (int)Magnitude.Hundred) + Others[0];
            number %= (int)Magnitude.Hundred;
        }
        if (number > 0)
        {
            if (words != "")
                words += " AND";

            if (number < 10)
                words += Units[number];
            else if (number < 20)
                words += IrregularTens[number - 10];
            else
            {
                words += Tens[number / 10 - 2];
                if ((number % 10) > 0)
                    words += "-" + Units[number % 10];
            }
        }

        return words;
    }

    public string ConvertFloatingPointToWords(int number)
    {
        if (number == 0)
            return "";

        return ConvertIntegersToWords(number);
    }

    public enum Magnitude
    {
        Hundred = 100,
        Thousand = 1000,
        Million = 1000000,
        Billion = 1000000000
    }
}