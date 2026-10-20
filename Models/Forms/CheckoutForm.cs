using System.ComponentModel.DataAnnotations;

namespace SlowBurn.Models;

public class CheckoutForm
{
    [Required(ErrorMessage = "Tell us who the order is for.")]
    [StringLength(60, ErrorMessage = "That name is too long (60 characters max).")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "We need an email for the receipt.")]
    [EmailAddress(ErrorMessage = "That doesn't look like a valid email address.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "A phone number helps the courier reach you.")]
    [Phone(ErrorMessage = "Use digits, spaces, + or - only.")]
    public string Phone { get; set; } = "";

    public DeliveryMethod Delivery { get; set; } = DeliveryMethod.Standard;

    [RequiredUnlessPickup(ErrorMessage = "Enter the street address.")]
    [StringLength(120, ErrorMessage = "Keep the address under 120 characters.")]
    public string Address { get; set; } = "";

    [RequiredUnlessPickup(ErrorMessage = "Enter the city.")]
    public string City { get; set; } = "";

    [RequiredUnlessPickup(ErrorMessage = "Enter the postal code.")]
    [RegularExpression(@"^\d{4,6}$", ErrorMessage = "Postal code should be 4 to 6 digits.")]
    public string PostalCode { get; set; } = "";

    [StringLength(200, ErrorMessage = "Keep the note under 200 characters.")]
    public string Note { get; set; } = "";

    [Range(typeof(bool), "true", "true", ErrorMessage = "Please accept the roast-to-order terms to continue.")]
    public bool AcceptTerms { get; set; }
}

/// <summary>Field is required unless the customer picked store pickup.</summary>
public class RequiredUnlessPickupAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        var pickup = context.ObjectInstance is CheckoutForm f && f.Delivery == DeliveryMethod.Pickup;
        if (pickup || !string.IsNullOrWhiteSpace(value as string)) return ValidationResult.Success;
        return new ValidationResult(ErrorMessage, new[] { context.MemberName! });
    }
}