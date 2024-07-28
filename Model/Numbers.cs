using System.ComponentModel.DataAnnotations;

namespace ConvertDigits.Model;

public class Numbers
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Dollars { get; set; } = null!;
    public string Cents { get; set; } = null!;
}