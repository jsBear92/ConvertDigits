using System.ComponentModel.DataAnnotations;

namespace ConvertDigits.Model;

public class Numbers
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Number { get; set; } = null!;
}