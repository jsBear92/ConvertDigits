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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Numbers>> PostNumber(Numbers number)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Create a new NumberConverter object
        NumberConverter converter = new();

        // Convert the string number(dollar and cents) to words
        Numbers words = converter.ConvertingWords(number.Dollars, number.Cents);

        // Add the converted number to the In-memory database
        _context.Numbers.Add(words);
        await _context.SaveChangesAsync();

        // Return the converted number to the client
        return CreatedAtAction(nameof(GetNumber), new { id = words.Id }, words);
    }
}

internal class NumberConverter
{
    // Array of words for units and tens
    private readonly string[] Units = {
        "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE"
    };
    private readonly string[] IrregularTens = {
        "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
    };
    private readonly string[] Tens = {
        "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
    };
    private readonly string[] Others = {
        "HUNDRED", "THOUSAND", "MILLION"
    };


    // Method to convert the string number(dollar and cents) to words
    public Numbers ConvertingWords(string dollar, string cents)
    {
        // Create a new Numbers object
        Numbers result = new ();
        
        // Parse the string number to integer
        int integerNumber = int.Parse(dollar);

        // Parse the string number to integer if not empty
        int fractionalNumber = cents != "" ? int.Parse(cents) : 0;

        // Convert the integer and fractional number to words
        result.Dollars = ConvertIntegersToWords(integerNumber);
        result.Cents = ConvertFloatingPointToWords(fractionalNumber);

        // Return the result
        return result;
    }

    // Method to convert the integer number to words
    public string ConvertIntegersToWords(int number)
    {
        // Create a string variable to store the words to return to the client
        string words = "";

        // Check if the number is zero
        // Return "ZERO" if the number is zero
        if (number == 0)
            return Units[0];

        //  Check if the number is negative
        // Add "MINUS" to the beginning of words
        if (number < 0)
            return "MINUS " + ConvertIntegersToWords(Math.Abs(number));

        // Check if the number is greater than million and than modulate the number
        // Add "MILLION" to the end of words
        if ((number / 1000000) > 0)
        {
            words += ConvertIntegersToWords(number / (int)Magnitude.Million) + " " + Others[2];
            number %= (int)Magnitude.Million;
        }

        // Check if the number is greater than thousand and than modulate the number
        // Add "THOUSAND" to the end of words
        if ((number / 1000) > 0)
        {
            words += ConvertIntegersToWords(number / (int)Magnitude.Thousand) + " " + Others[1];
            number %= (int)Magnitude.Thousand;
        }

        // Check if the number is greater than hundred and than modulate the number
        // Add "HUNDRED" to the end of words
        if ((number / 100) > 0)
        {
            words += ConvertIntegersToWords(number / (int)Magnitude.Hundred) + " " + Others[0];
            number %= (int)Magnitude.Hundred;
        }

        // Calculate between 1 and 99
        if (number > 0)
        {
            // Add "AND" to the end of words if the words is not empty
            // Such as "ONE HUNDRED AND TWENTY-THREE"
            if (words != "")
                words += " AND ";

            // Check if the number is less than 10
            // Add the unit words to the end of words
            if (number < 10)
                words += Units[number];

            // Check if the number is less than 20
            // Add the tens words (11-19) to the end of words
            else if (number < 20)
                words += IrregularTens[number - 10];
            
            // Check if the number is greater than 20
            // Add the tens words (20-90) to the end of words
            // Then add hyphen and the unit words to the end of words
            else
            {
                words += Tens[number / 10 - 2];
                if ((number % 10) > 0)
                    words += "-" + Units[number % 10];
            }
        }

        return words;
    }

    // Method to convert the fractional number to words
    public string ConvertFloatingPointToWords(int number)
    {
        if (number == 0)
            return "";

        return ConvertIntegersToWords(number);
    }

    // Preset values for magnitude
    public enum Magnitude
    {
        Hundred = 100,
        Thousand = 1000,
        Million = 1000000
    }
}