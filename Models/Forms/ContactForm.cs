using System.ComponentModel.DataAnnotations;

namespace SlowBurn.Models;

/// <summary>Bound to the EditForm on Contact.razor. Keep validation messages human.</summary>
public class ContactForm
{
    [Required(ErrorMessage = "Please tell us your name.")]
    [StringLength(60, ErrorMessage = "Name is too long.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "That doesn't look like a valid email.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Write a short message.")]
    [MinLength(10, ErrorMessage = "Message must be at least 10 characters.")]
    public string Message { get; set; } = string.Empty;
}
