using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ConvertDigits.Model;

public class Numbers
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Dollars { get; set; } = null!;
    [MaybeNull]
    public string Cents { get; set; }
}